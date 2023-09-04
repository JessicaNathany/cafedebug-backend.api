using cafedebug_backend.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cafedebug_backend.infrastructure.ConfigurationModelBuilder
{
    public class EpisodeTagModelBuilder : IEntityTypeConfiguration<EpisodeTag>
    {
        public void Configure(EntityTypeBuilder<EpisodeTag> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(ep => ep.Episode)
                .WithMany(et => et.EpisodiesTags)
                .HasForeignKey(e => e.EpisodeId);

            builder.HasOne(ep => ep.Tag)
                .WithMany(et => et.EpisodesTags)
                .HasForeignKey(e => e.TagId);

            builder.ToTable("EpisodeTag");
        }
    }
}
