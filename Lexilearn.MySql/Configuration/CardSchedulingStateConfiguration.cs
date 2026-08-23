using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexilearn.MySql.Configuration
{
    public class CardSchedulingStateConfiguration : IEntityTypeConfiguration<CardSchedulingState>
    {
        public void Configure(EntityTypeBuilder<CardSchedulingState> builder)
        {
            builder.Property(e => e.Status).HasDefaultValue(SchedulingStatus.New);
            builder.Property(e => e.ReviewCount).HasDefaultValue(0);
            builder.Property(e => e.LapseCount).HasDefaultValue(0);
            builder.Property(e => e.IntervalDays).HasDefaultValue(0);

            builder.HasIndex(e => new { e.Status, e.NextReviewAt });
        }
    }
}
