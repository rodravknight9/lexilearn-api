using Lexilearn.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexilearn.MySql.Configuration
{
    public class StudySessionSettingsConfiguration : IEntityTypeConfiguration<StudySessionSettings>
    {
        public void Configure(EntityTypeBuilder<StudySessionSettings> builder)
        {
            builder
                .HasOne<Deck>()
                .WithMany()
                .HasForeignKey(e => e.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.DeckId).IsUnique();

            builder.Property(e => e.SessionSize).HasDefaultValue(20);
            builder.Property(e => e.NewCardsPercentage).HasDefaultValue(20);
            builder.Property(e => e.HardCardsPercentage).HasDefaultValue(30);
        }
    }
}
