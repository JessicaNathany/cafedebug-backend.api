using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder
{
    public class AdvertisementModelBuilder : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(a => a.Description)
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder.Property(a => a.Url)
                .HasColumnType("varchar(250)");

            builder.Property(a => a.ImageUrl)
                .HasColumnType("varchar(250)");

            builder.Property(a => a.StartDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.EndDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.Active)
                .HasColumnType("bit");

            builder.Property(a => a.AdvertisementType)
                .HasColumnType("bit");

            builder.ToTable("Advertisement");
        }
    }
}
