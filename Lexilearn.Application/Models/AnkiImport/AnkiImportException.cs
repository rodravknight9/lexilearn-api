namespace Lexilearn.Application.Models.AnkiImport
{
    public class AnkiImportException : Exception
    {
        public AnkiImportException(string message) : base(message)
        {
        }

        public AnkiImportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
