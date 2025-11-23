using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Shared;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Setups;

public class BannerRepositoryMockSetup(Mock<IBannerRepository> bannerRepository)
{
    public void BannerExists()
    {
        bannerRepository
            .Setup(x=> x.AnyAsync(It.IsAny<Expression<Func<Banner, bool>>>()))
            .ReturnsAsync(true);
    }

    public void BannerDoesNotExist()
    {
        bannerRepository.Setup(x=> x.AnyAsync(It.IsAny<Expression<Func<Banner, bool>>>()))
            .ReturnsAsync(false);
    }  
    
    public void GetBannerById(Banner banner)
    {
        bannerRepository
            .Setup(x=> x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(banner);
    }

    public void GetBannerByIdNotFound(int bannerId)
    {
        bannerRepository
            .Setup(x=> x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Banner?)null);
    }

    public void BannerSave(Action<Banner> callback)
    {
        bannerRepository
            .Setup(x=> x.SaveAsync(It.IsAny<Banner>()))
            .Callback(callback)
            .Returns(Task.CompletedTask);
    }

    public void BannerSaveThrows(Exception exception)
    {
        bannerRepository
            .Setup(x => x.SaveAsync(It.IsAny<Banner>()))
            .ThrowsAsync(exception);
    }

    public void BannerUpdate()
    {
        bannerRepository
            .Setup(x=> x.UpdateAsync(It.IsAny<Banner>()))
            .Returns(Task.CompletedTask);
    }

    public void BannerGetPageList(IPagedResult<Banner> pagedResult, PageRequest pageRequest)
    {
        bannerRepository
            .Setup(x => x.GetPageList(pageRequest.Page, pageRequest.PageSize, pageRequest.SortBy, pageRequest.Descending, CancellationToken.None))
            .ReturnsAsync(pagedResult);
    }
}

