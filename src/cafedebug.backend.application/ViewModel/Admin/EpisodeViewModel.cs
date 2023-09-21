using cafedebug_backend.domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace cafedebug.backend.application.Admin
{
    public class EpisodeViewModel
    {
        [JsonProperty("titulo")]
        public string Title { get; set; }

        [JsonProperty("descricao")]
        public string Description { get; set; }

        [JsonProperty("descricaoResumo")]
        public string ResumeDescription { get; set; }

        [JsonProperty("url")] 
        public string Url { get; set; }

        [JsonProperty("imagemUrl")]
        public string ImageUrl { get; set; }
        
        [NotMapped]
        public IFormFile ImageUpload { get; set; }

        [JsonProperty("tagsEpisodio")]
        public IList<EpisodeTag> EpisodiesTags { get; set; }

        public DateTime? PublicationDate { get; set; }
        public bool Active { get; set; }

        [JsonProperty("numeroEpisodio")]
        public int Number { get; set; }

        [JsonProperty("idCategoria")]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }

        public int? View { get; set; }
        
        public int? Like { get; set; }
    }
}
