using cafedebug_backend.domain.Episodes.Repositories;
using Moq;

namespace cafedebug.backend.api.test.Shared.Verifications.Podcasts;

public class CategoryRepositoryVerifications(Mock<ICategoryRepository> categoryRepository)
{
    public void VerifyCategoryRetrieved(int categoryId, Times times)
    {
        categoryRepository.Verify(x => x.GetByIdAsync(categoryId), times);
    }
}