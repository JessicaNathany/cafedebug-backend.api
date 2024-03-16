namespace cafedebug_backend.domain.Entities
{
    public class News : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int ImageId { get; set; }

        public string UrlImage { get; set; }

        public string NewsLink { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
