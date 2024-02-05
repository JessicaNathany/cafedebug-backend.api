namespace cafedebug.backend.application.ViewModel
{
    public class TagViewModel
    {
        public int Id { get; set; }

        public Guid Code { get; set; }
        public string Name { get; set; }

        public IList<EpisodeTagViewModel> EpisodesTags { get; set; }
    }
}
