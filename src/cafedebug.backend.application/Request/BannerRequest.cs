using Newtonsoft.Json;

namespace cafedebug_backend.application.Request
{
    public class BannerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string UrlImage { get; set; }

        public string Url { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? DateUpdate { get;  set; }
        public bool Active { get; set; }
    }
}
