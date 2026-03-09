using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Setups.Podcasts;

public class CategoryRepositoryMockSetup(Mock<ICategoryRepository> categoryRepository)
{
    public void CategoryExists(Category category)
    {
        categoryRepository
            .Setup(x => x.GetByIdAsync(category.Id))
            .ReturnsAsync(category);
    }

    public void CategoryExists()
    {
        categoryRepository
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(true);
    }

    public void CategoryDoesNotExist()
    {
        categoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
           .ReturnsAsync(false);
    }

    public void CategoryDoesNotExist(int categoryId)
    {
        categoryRepository.Setup(x => x.GetByIdAsync(categoryId));
    }
    public void GetCategoryById(Category banner)
    {
        categoryRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(banner);
    }
    public void GetCategoryByIdNotFound(int bannerId)
    {
        categoryRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);
    }

    public void CategorySave(Action<Category> callback)
    {
        categoryRepository
            .Setup(x => x.SaveAsync(It.IsAny<Category>()))
            .Callback(callback)
            .Returns(Task.CompletedTask);
    }

    public void CategoryUpdate()
    {
        categoryRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);
    }

    public void CategorySaveThrows(Exception exception)
    {
        categoryRepository
            .Setup(x => x.SaveAsync(It.IsAny<Category>()))
            .ThrowsAsync(exception);
    }
    public void CategoryGetPageList(IPagedResult<Category> pagedResult, PageRequest pageRequest)
    {
        categoryRepository
            .Setup(x => x.GetPageList(pageRequest.Page, pageRequest.PageSize, pageRequest.SortBy, pageRequest.Descending, CancellationToken.None))
            .ReturnsAsync(pagedResult);
    }
}