using cafedebug_backend.domain.Enums;

namespace cafedebug_backend.domain.Entities
{
    /// <summary>
    /// Essa entidade é sobre Anúncios, podendo ter anúncios de produtos, serviço ou divulgação de vagas
    /// </summary>
    public class Advertisement : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public AdvertisementType AdvertisementType { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }
    }
}
