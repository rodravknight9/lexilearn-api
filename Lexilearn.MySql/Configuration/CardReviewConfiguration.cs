using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexilearn.MySql.Configuration
{
    public class CardReviewConfiguration : IEntityTypeConfiguration<CardReview>
    {
        public void Configure(EntityTypeBuilder<CardReview> builder)
        {
            builder.ToTable("CardReviews");

            builder.Property(e => e.ReviewedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            builder.Property(e => e.PreviousStatus).HasDefaultValue(SchedulingStatus.New);
            builder.Property(e => e.NextStatus).HasDefaultValue(SchedulingStatus.New);
        }
    }
}
