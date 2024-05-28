using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder
{
    public class EpisodeModelBuilder : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType("varchar(65535)");

            builder.Property(e => e.ResumeDescription)
                .IsRequired()
                .HasColumnType("varchar(65535)");

            builder.Property(e => e.Url)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(e => e.ImageUrl)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(e => e.ResumeDescription)
                .HasColumnType("varchar(65535)");

            builder.Property(e => e.PublicationDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.UpdateDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(e => e.Active)
                .HasColumnType("bit");

            builder.Property(e => e.Number)
                .HasColumnType("int");

            builder.Property(e => e.CategoryId)
                .HasColumnType("int");

            builder.Property(e => e.View)
               .HasColumnType("int");

            builder.Property(e => e.Like)
               .HasColumnType("int");

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(e => e.CategoryId);

            builder.ToTable("Episode");
        }
    }
}
