using cafedebug.backend.application.ViewModel;
using Microsoft.AspNetCore.Http;

namespace cafedebug_backend.application.Request
{
    public class EpisodeRequest
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResumeDescription { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageUpload { get; set; }
        public IList<EpisodeViewModel> EpisodiesTags { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool Active { get; set; }
        public int Number { get; set; }
        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
        public int? View { get; set; }
        public int? Like { get; set; }
    }
}
