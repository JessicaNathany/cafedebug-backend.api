using cafedebug_backend.domain.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder
{
    public class NewsModelBuilder : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasColumnType("varchar(80)");

            builder.Property(b => b.Description)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(b => b.ImageId)
                .IsRequired();

            builder.Property(b => b.NewsLink)
               .IsRequired()
               .HasColumnType("varchar(200)");

            builder.Property(b => b.PublicationDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.UpdateDate)
             .IsRequired()
             .HasColumnType("datetime");

            builder.ToTable("News");
        }
    }
}
