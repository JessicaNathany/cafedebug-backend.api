using cafedebug_backend.domain.Podcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Data.ConfigurationModelBuilder;

public class CategoryModelBuilder : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired()
            .HasColumnType("char(36)");       

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.ToTable("Category");
    }   
}