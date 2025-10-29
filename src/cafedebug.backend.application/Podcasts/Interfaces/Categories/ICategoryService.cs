using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.application.Podcasts.Interfaces.Categories;

public interface ICategoryService
{
    Task Save(Category category);

    Task Update(Category category);

    Task Delete(Guid code);
}