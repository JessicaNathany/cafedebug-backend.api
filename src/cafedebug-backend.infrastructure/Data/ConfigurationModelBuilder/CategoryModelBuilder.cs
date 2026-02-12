using cafedebug_backend.domain.Podcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Data.ConfigurationModelBuilder;

public class CategoryModelBuilder : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(c => c.UpdatedAt)
            .HasColumnType("datetime");

        builder.ToTable("Category");
    }   
}