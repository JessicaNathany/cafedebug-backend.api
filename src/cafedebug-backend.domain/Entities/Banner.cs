namespace cafedebug_backend.domain.Entities
{
    public class Banner : Entity
    {
        public string Name { get; set; }

        public string UrlImage { get; set; }

        public string Url { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Active { get; set; }
    }
}
