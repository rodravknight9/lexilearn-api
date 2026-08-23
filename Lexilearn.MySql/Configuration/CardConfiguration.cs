using Lexilearn.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Lexilearn.MySql.Configuration
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder
                .HasOne(e => e.Deck)
                .WithMany(t => t.Cards)
                .HasForeignKey(e => e.DeckId)
                .IsRequired(true);

            builder
                .HasOne(e => e.SchedulingState)
                .WithOne(s => s.Card)
                .HasForeignKey<CardSchedulingState>(s => s.CardId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.ImportSource).HasMaxLength(30);
            builder.Property(e => e.ExternalId).HasMaxLength(100);

            builder
                .HasIndex(e => new { e.ImportSource, e.ExternalId })
                .IsUnique();
        }
    }
}
