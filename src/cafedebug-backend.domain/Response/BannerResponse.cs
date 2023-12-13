namespace cafedebug_backend.domain.Response
{
    public class BannerResponse
    {
        public int Id { get; set; }

        public Guid Code { get; set; }
        public string Name { get; set; }

        public string UrlImage { get; set; }

        public string Url { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool Active { get; set; }
    }
}
