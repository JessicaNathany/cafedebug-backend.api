﻿using cafedebug_backend.domain.Enums;
using Newtonsoft.Json;

namespace cafedebug.backend.application.ViewModel
{
    public class AdvertisementViewModel
    {
        [JsonProperty("titulo")]
        public string Title { get; set; }

        [JsonProperty("descricao")]
        public string Description { get; set; }

        [JsonProperty("dataInicial")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("dataFinal")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("tipoAnuncio")]
        public AdvertisementType AdvertisementType { get; set; }

        [JsonProperty("urlAnuncio")]
        public string Url { get; set; }

        public bool Active { get; set; }
    }
}
