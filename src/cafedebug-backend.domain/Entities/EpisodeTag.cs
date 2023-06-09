namespace cafedebug_backend.domain.Entities
{
    public class EpisodeTag : Entity
    {
        public int Id { get; set; }

        public int EpisodeId { get; set; }

        public Episode Episode { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
