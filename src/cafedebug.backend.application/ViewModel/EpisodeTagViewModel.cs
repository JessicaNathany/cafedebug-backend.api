namespace cafedebug.backend.application.ViewModel
{
    public class EpisodeTagViewModel
    {
        public int Id { get; set; }

        public int EpisodeId { get; set; }

        public EpisodeViewModel Episode { get; set; }

        public int TagId { get; set; }

        public TagViewModel Tag { get; set; }
    }
}
