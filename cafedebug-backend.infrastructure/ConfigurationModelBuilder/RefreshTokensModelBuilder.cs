using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder
{
    public class RefreshTokensModelBuilder : IEntityTypeConfiguration<RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Token)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.ExpirationDate)
                .IsRequired();

            builder.Property(r => r.IsActive)
                .IsRequired();

            builder.HasOne<UserAdmin>()
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder.ToTable("RefreshTokens");   
        }   
    }
}
