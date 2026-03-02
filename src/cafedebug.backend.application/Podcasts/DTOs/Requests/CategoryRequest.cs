using cafedebug_backend.domain.Podcasts;
namespace cafedebug.backend.application.Podcasts.DTOs.Requests;

public class CategoryRequest
{
    public string Name { get; init; }

    public Category ToCategory()
    {
        return new Category(Name);
    }
}

