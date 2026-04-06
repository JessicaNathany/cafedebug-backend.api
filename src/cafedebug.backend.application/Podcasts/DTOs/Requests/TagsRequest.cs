using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.DTOs.Requests
{
    public class TagsRequest
    {
        public string Name { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public Tags ToTags()
        {
            return new Tags(Name);
        }
    }
}
