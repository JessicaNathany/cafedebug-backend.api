namespace cafedebug_backend.domain.Entities
{
    public class Banner : Entity
    {
        public string Name { get; private set; }

        public string UrlImage { get; private set; }

        public string? Url { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public DateTime? UpdateDate { get; private set; }

        public bool Active { get; private set; }

        public Banner(string name, string urlImage, string url, DateTime startDate, DateTime endDate, bool active)
        {
            Code = Guid.NewGuid();
            Name = name;
            UrlImage = urlImage;
            Url = url;
            StartDate = startDate;
            EndDate = endDate;
            Active = active;
            EndDateVerify(endDate);
        }

        public void Update(string name, string urlImage, string url, DateTime startDate, DateTime endDate, DateTime? updateDate, bool active)
        {
            Name = name;
            UrlImage = urlImage;
            Url = url;
            StartDate = startDate;
            EndDate = endDate;
            UpdateDate = updateDate;
            Active = active;
            EndDateVerify(endDate);
        }
        public void EndDateVerify(DateTime endDate)
        {
            if (endDate == DateTime.Now.AddDays(-1))
                Active = false;
        }
    }
}
