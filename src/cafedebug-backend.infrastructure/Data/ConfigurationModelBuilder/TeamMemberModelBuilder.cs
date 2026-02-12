using cafedebug_backend.domain.Podcasts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Data.ConfigurationModelBuilder;

public class TeamMemberModelBuilder : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");
        
        builder.Property(t => t.Nickname)
            .HasColumnType("varchar(50)");
        
        builder.Property(t => t.Email)
            .HasColumnType("varchar(150)");
        
        builder.Property(t => t.Bio)
            .HasColumnType("varchar(1000)");
        
        builder.Property(t => t.PodcastRole)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(t => t.GitHubUrl)
            .HasColumnType("varchar(500)");

        builder.Property(t => t.InstagramUrl)
            .HasColumnType("varchar(500)");

        builder.Property(t => t.LinkedInUrl)
            .HasColumnType("varchar(500)");

        builder.Property(t => t.ProfilePhotoUrl)
            .HasColumnType("varchar(500)");

        builder.Property(t => t.JobTitle)
            .HasColumnType("varchar(100)");
        
        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(true);
        
        builder.Property(t => t.JoinedAt)
            .IsRequired()
            .HasColumnType("datetime");
        
        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType("datetime");

        builder.ToTable("TeamMember");
    }
}