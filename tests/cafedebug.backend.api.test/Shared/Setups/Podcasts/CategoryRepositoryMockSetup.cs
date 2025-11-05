using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Podcasts;

public class CategoryRepositoryMockSetup(Mock<ICategoryRepository> categoryRepository)
{
    public void CategoryExists(Category category)
    {
        categoryRepository
            .Setup(x => x.GetByIdAsync(category.Id))
            .ReturnsAsync(category);
    }

    public void CategoryDoesNotExist(int categoryId)
    {
        categoryRepository
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);
    }
}