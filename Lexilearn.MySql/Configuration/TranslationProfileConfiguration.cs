using Lexilearn.Domain;
using Lexilearn.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lexilearn.MySql.Configuration;

public class TranslationProfileConfiguration : IEntityTypeConfiguration<TranslationProfile>
{
    public void Configure(EntityTypeBuilder<TranslationProfile> builder)
    {
        builder.HasIndex(e => e.UserId);

        builder.Property(e => e.Name).HasMaxLength(120).IsRequired();
        builder.Property(e => e.Kind).HasDefaultValue(TranslationProfileKind.Custom);
        builder.Property(e => e.Url).HasMaxLength(2000).IsRequired();
        builder.Property(e => e.HttpMethod).HasMaxLength(10).IsRequired().HasDefaultValue("POST");
        builder.Property(e => e.HeadersJson).IsRequired();
        builder.Property(e => e.BodyJson).IsRequired();
        builder.Property(e => e.ResponsePath).HasMaxLength(300).IsRequired();
        builder.Property(e => e.IsDefault).HasDefaultValue(false);
    }
}
