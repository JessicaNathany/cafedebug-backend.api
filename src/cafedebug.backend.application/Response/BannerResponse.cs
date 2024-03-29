namespace cafedebug_backend.application.Response
{
    public class BannerResponse
    {
        public int Id { get; set; }

        public Guid Code { get; set; }

        public string Name { get; set; }

        public string UrlImage { get; set; }

        public string Url { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string UpdateDate { get; set; }

        public string Active { get; set; }
    }
}
