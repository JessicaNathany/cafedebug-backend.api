using System.Linq.Expressions;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Pagination;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Banner;

public class BannerRepositoryMockSetup(Mock<IBannerRepository> bannerRepository)
{
    public void BannerExists()
    {
        bannerRepository
            .Setup(x=> x.AnyAsync(It.IsAny<Expression<Func<cafedebug_backend.domain.Banners.Banner, bool>>>()))
            .ReturnsAsync(true);
    }

    public void BannerDoesNotExist()
    {
        bannerRepository.Setup(x=> x.AnyAsync(It.IsAny<Expression<Func<cafedebug_backend.domain.Banners.Banner, bool>>>()))
            .ReturnsAsync(false);
    }  
    
    public void GetBannerById(cafedebug_backend.domain.Banners.Banner banner)
    {
        bannerRepository
            .Setup(x=> x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(banner);
    }

    public void GetBannerByIdNotFound(int bannerId)
    {
        bannerRepository
            .Setup(x=> x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((cafedebug_backend.domain.Banners.Banner?)null);
    }

    public void BannerSave(Action<cafedebug_backend.domain.Banners.Banner> callback)
    {
        bannerRepository
            .Setup(x=> x.SaveAsync(It.IsAny<cafedebug_backend.domain.Banners.Banner>()))
            .Callback(callback)
            .Returns(Task.CompletedTask);
    }

    public void BannerSaveThrows(Exception exception)
    {
        bannerRepository
            .Setup(x => x.SaveAsync(It.IsAny<cafedebug_backend.domain.Banners.Banner>()))
            .ThrowsAsync(exception);
    }

    public void BannerUpdate()
    {
        bannerRepository
            .Setup(x=> x.UpdateAsync(It.IsAny<cafedebug_backend.domain.Banners.Banner>()))
            .Returns(Task.CompletedTask);
    }

    public void BannerGetPageList(IPagedResult<cafedebug_backend.domain.Banners.Banner> pagedResult, PageRequest pageRequest)
    {
        bannerRepository
            .Setup(x => x.GetPageList(pageRequest.Page, pageRequest.PageSize, pageRequest.SortBy, pageRequest.Descending, CancellationToken.None))
            .ReturnsAsync(pagedResult);
    }
}

