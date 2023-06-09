namespace cafedebug_backend.domain.Entities
{
    public class Tag : Entity
    {
        public string Name { get; set; }

        public IList<EpisodeTag> EpisodesTags { get; set; }
    }
}
