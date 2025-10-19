using System.Text.Json;
using cafedebug_backend.domain.Episodes.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder;

public class EpisodeModelBuilder : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasColumnType("char(36)");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasColumnType("mediumtext");

        builder.Property(e => e.ShortDescription)
            .IsRequired()
            .HasColumnType("varchar(500)");

        builder.Property(e => e.Url)
            .IsRequired()
            .HasColumnType("varchar(2000)");

        builder.Property(e => e.ImageUrl)
            .IsRequired()
            .HasColumnType("varchar(2000)");

        builder.Property(e => e.Tags)
            .HasColumnType("json")
            .HasConversion(
                tags => JsonSerializer.Serialize(tags, (JsonSerializerOptions)null),
                tags => JsonSerializer.Deserialize<List<string>>(tags, (JsonSerializerOptions)null).AsReadOnly()
            );

        builder.Property(e => e.PublishedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(e => e.Active)
            .HasColumnType("tinyint(1)");

        builder.Property(e => e.Number)
            .HasColumnType("int");

        builder.Property(e => e.CategoryId)
            .HasColumnType("int");

        builder.Property(e => e.Views)
            .HasColumnType("int");

        builder.Property(e => e.Likes)
            .HasColumnType("int");

        builder.HasOne( e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.ToTable("Episode");
    }
}