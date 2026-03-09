using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Verifications.Podcasts;
public class CategoryRepositoryVerifications(Mock<ICategoryRepository> categoryRepository)
{
    public void VerifyCategoryExistenceChecked(Times times)
    {
        categoryRepository.Verify(b => b.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()), times);
    }

    public void VerifyCategoryRetrieved(int categoryId, Times times)
    {
        categoryRepository.Verify(x => x.GetByIdAsync(categoryId), times);
    }

    public void VerifyCategorySaved(Times times)
    {
        categoryRepository.Verify(x => x.SaveAsync(It.IsAny<Category>()), times);
    }
    public void VerifyCategoryUpdated(Times times)
    {
        categoryRepository.Verify(x => x.UpdateAsync(It.IsAny<Category>()), times);
    }
    public void VerifyCategoryDeleted(Category category, Times once)
    {
        categoryRepository.Verify(x => x.DeleteAsync(category), once);
    }

    public void VerifyCategoryPageListRetrieved(int page, int pageSize,
        string? sortBy, bool descending, Times times)
    {
        categoryRepository.Verify(x => x.GetPageList(page, pageSize, sortBy, descending, CancellationToken.None), times);
    }
}