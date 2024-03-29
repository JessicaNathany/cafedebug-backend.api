using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace cafedebug_backend.infrastructure.Context
{
    /// <summary>
    /// Represents the application's data context, used for interacting with the database.
    /// </summary>
    public class CafedebugContext : DbContext
    {
        public CafedebugContext(DbContextOptions<CafedebugContext> options) : base(options)
        { }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<UserAdmin> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CafedebugContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
