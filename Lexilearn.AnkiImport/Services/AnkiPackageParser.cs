using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Lexilearn.Application.Contracts.Infastructure;
using Lexilearn.Application.Models.AnkiImport;
using Microsoft.Data.Sqlite;

namespace Lexilearn.AnkiImport.Services
{
    public class AnkiPackageParser : IAnkiPackageParser
    {
        private const char FieldSeparator = '\u001f';

        private static readonly Regex SoundRefPattern = new(@"\[sound:[^\]]*\]", RegexOptions.Compiled);
        private static readonly Regex ImgTagPattern = new(@"<img\b[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex HtmlTagPattern = new(@"<[^>]+>", RegexOptions.Compiled);

        public async Task<AnkiPackage> ParseAsync(Stream apkgStream, CancellationToken cancellationToken)
        {
            var zipPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.apkg");
            var sqlitePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.sqlite");

            try
            {
                await using (var zipFile = File.Create(zipPath))
                {
                    await apkgStream.CopyToAsync(zipFile, cancellationToken);
                }

                using var archive = ZipFile.OpenRead(zipPath);
                var collectionEntry = archive.GetEntry("collection.anki21") ?? archive.GetEntry("collection.anki2");
                if (collectionEntry is null)
                    throw new AnkiImportException("The file is not a valid Anki package (no collection database found).");

                collectionEntry.ExtractToFile(sqlitePath, overwrite: true);

                return ReadPackage(sqlitePath);
            }
            catch (InvalidDataException ex)
            {
                throw new AnkiImportException("The file is not a valid .apkg archive.", ex);
            }
            catch (SqliteException ex)
            {
                throw new AnkiImportException("The Anki collection database could not be read.", ex);
            }
            finally
            {
                TryDelete(zipPath);
                TryDelete(sqlitePath);
            }
        }

        private AnkiPackage ReadPackage(string sqlitePath)
        {
            using var connection = new SqliteConnection($"Data Source={sqlitePath};Mode=ReadOnly");
            connection.Open();

            var deckNames = TableExists(connection, "decks")
                ? ReadDeckNamesFromTable(connection)
                : ReadDeckNamesFromColJson(connection);

            var noteFields = ReadNoteFields(connection);

            var skippedAttachments = 0;
            string CleanField(string raw)
            {
                skippedAttachments += SoundRefPattern.Matches(raw).Count;
                skippedAttachments += ImgTagPattern.Matches(raw).Count;
                var withoutSound = SoundRefPattern.Replace(raw, string.Empty);
                var withoutHtml = HtmlTagPattern.Replace(withoutSound, string.Empty);
                return WebUtility.HtmlDecode(withoutHtml).Trim();
            }

            var decksByName = new Dictionary<string, AnkiDeck>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, nid, did FROM cards;";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var cardId = reader.GetInt64(0);
                    var noteId = reader.GetInt64(1);
                    var deckId = reader.GetInt64(2);

                    if (!noteFields.TryGetValue(noteId, out var fields) || fields.Length == 0)
                        continue;

                    var deckName = deckNames.TryGetValue(deckId, out var name) ? name : "Imported deck";
                    if (!decksByName.TryGetValue(deckName, out var deck))
                    {
                        deck = new AnkiDeck { Name = deckName };
                        decksByName[deckName] = deck;
                    }

                    var front = CleanField(fields[0]);
                    var back = fields.Length > 1 ? CleanField(fields[1]) : string.Empty;

                    deck.Notes.Add(new AnkiNote
                    {
                        CardExternalId = cardId.ToString(),
                        Front = front,
                        Back = back,
                    });
                }
            }

            return new AnkiPackage
            {
                Decks = decksByName.Values.Where(d => d.Notes.Count > 0).ToList(),
                SkippedAttachmentCount = skippedAttachments,
            };
        }

        private static Dictionary<long, string[]> ReadNoteFields(SqliteConnection connection)
        {
            var result = new Dictionary<long, string[]>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, flds FROM notes;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var noteId = reader.GetInt64(0);
                var flds = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                result[noteId] = flds.Split(FieldSeparator);
            }
            return result;
        }

        private static Dictionary<long, string> ReadDeckNamesFromTable(SqliteConnection connection)
        {
            var result = new Dictionary<long, string>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name FROM decks;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result[reader.GetInt64(0)] = reader.GetString(1);
            }
            return result;
        }

        private static Dictionary<long, string> ReadDeckNamesFromColJson(SqliteConnection connection)
        {
            var result = new Dictionary<long, string>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT decks FROM col LIMIT 1;";
            var json = command.ExecuteScalar() as string;
            if (string.IsNullOrWhiteSpace(json))
                return result;

            using var document = JsonDocument.Parse(json);
            foreach (var property in document.RootElement.EnumerateObject())
            {
                if (property.Value.TryGetProperty("id", out var idElement) &&
                    property.Value.TryGetProperty("name", out var nameElement))
                {
                    result[idElement.GetInt64()] = nameElement.GetString() ?? "Imported deck";
                }
            }
            return result;
        }

        private static bool TableExists(SqliteConnection connection, string tableName)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$name;";
            command.Parameters.AddWithValue("$name", tableName);
            return command.ExecuteScalar() is not null;
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (IOException)
            {
                // Best-effort cleanup only.
            }
        }
    }
}
