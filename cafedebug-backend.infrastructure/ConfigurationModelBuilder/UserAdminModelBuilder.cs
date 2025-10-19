using cafedebug_backend.domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.Mapping
{
    public class UserAdminModelBuilder : IEntityTypeConfiguration<UserAdmin>
    {
        public void Configure(EntityTypeBuilder<UserAdmin> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(b => b.Email)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(b => b.HashedPassword)
                .IsRequired()
                .HasColumnType("varchar(500)");
            builder.ToTable("UserAdmin");
        }
    }
}
