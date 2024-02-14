using Microsoft.AspNetCore.Http;

namespace cafedebug_backend.domain.Entities
{
    public class Episode : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ResumeDescription { get; private set; }
        public string Url { get; private set; }
        public string ImageUrl { get; private set; }

        public IFormFile ImageUpload { get; private set; }
        public IList<EpisodeTag> EpisodiesTags { get; private set; }
        public DateTime PublicationDate { get; private set; }

        public DateTime? UpdateDate { get; private set; }
        public bool Active { get; private set; }
        public int Number { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public int? View { get; private set; }
        public int? Like { get; private set; }

        public Episode(
            string title,
            string description,
            string resumeDescription,
            string url,
            string imageUrl,
            DateTime publicationDate,
            bool active,
            int number,
            int categoryId,
            int? view,
            int? like)
        {
            Title = title;
            Description = description;
            ResumeDescription = resumeDescription;
            Url = url;
            ImageUrl = imageUrl;
            PublicationDate = publicationDate;
            Active = active;
            Number = number;
            CategoryId = categoryId;
            View = 0;
            Like = 0;
            EndDateVerify(publicationDate);
        }

        public void Update(
            string title,
            string description,
            string resumeDescription,
            string url,
            string imageUrl,
            DateTime publicationDate,
            bool active,
            int number,
            int categoryId,
            int? view,
            int? like)
        {
            Title = title;
            Description = description;
            ResumeDescription = resumeDescription;
            Url = url;
            ImageUrl = imageUrl;
            PublicationDate = publicationDate;
            Active = active;
            Number = number;
            CategoryId = categoryId;
            View = view;
            Like = like;
        }

        public void EndDateVerify(DateTime endDate)
        {
            if (endDate == DateTime.Now.AddDays(-1))
                Active = false;
        }
    }
}
