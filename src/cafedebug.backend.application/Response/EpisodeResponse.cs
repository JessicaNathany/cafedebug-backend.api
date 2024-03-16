using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.application.Response
{
    public class EpisodeResponse
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResumeDescription { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string PublicationDate { get; set; }
        public string Active { get; set; }
        public string Number { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public string View { get; set; }
        public string Like { get; set; }
    }
}
