using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Mapping
{
    public class BannerModelBuilder : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(b => b.UrlImage)
                .HasColumnType("varchar(500)")
                .IsRequired();

            builder.Property(b => b.Url)
                .HasColumnType("varchar(300)");

            builder.Property(b => b.StartDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.EndDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.UpdateDate)
                .HasColumnType("datetime");

            builder.Property(b => b.Active)
                .HasColumnType("bit");

            builder.Property(b => b.Ordem)
                .HasColumnType("int");

            builder.ToTable("Banner");
        }
    }
}
