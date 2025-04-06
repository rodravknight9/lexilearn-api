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
        }
    }
}
