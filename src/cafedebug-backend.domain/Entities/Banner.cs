﻿namespace cafedebug_backend.domain.Entities
{
    public class Banner : Entity
    {
        public string Name { get; private set; }

        public string UrlImage { get; private set; }

        public string Url { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public DateTime? UpdateDate { get; set; }

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
            Disable(endDate);
        }
       
        public void Disable(DateTime endDate)
        {
            if(endDate == DateTime.Now.AddDays(-1))
            Active = false;
        }
    }
}
