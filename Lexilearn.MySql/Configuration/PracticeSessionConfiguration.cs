using Lexilearn.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexilearn.MySql.Configuration;

public class PracticeSessionConfiguration : IEntityTypeConfiguration<PracticeSession>
{
    public void Configure(EntityTypeBuilder<PracticeSession> builder)
    {
        builder
            .HasMany(s => s.Cards)
            .WithOne(r => r.PracticeSession)
            .HasForeignKey(r => r.SessionId)
            .IsRequired();
    }
}