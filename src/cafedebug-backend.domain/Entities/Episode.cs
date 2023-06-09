using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace cafedebug_backend.domain.Entities
{
    public class Episode : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ResumeDescription { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile ImageUpload { get; set; }
        public IList<EpisodeTag> EpisodiesTags { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool Active { get; set; }
        public int Number { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? View { get; set; }
        public int? Like { get; set; }
    }
}
