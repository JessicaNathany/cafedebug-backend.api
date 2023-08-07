using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace cafedebug_backend.infrastructure.Context
{
    public class CafedebugContext : DbContext
    {
        public CafedebugContext(DbContextOptions<CafedebugContext> options) : base(options)
        { }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EpisodeTag> TagsEpisode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CafedebugContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
