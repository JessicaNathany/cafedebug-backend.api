using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Banners.Repositories;
using Moq;
using System.Linq.Expressions;

namespace cafedebug.backend.api.test.Shared.Verifications
{
    public class BannerRepositoryVerification(Mock<IBannerRepository> bannerRepository)
    {
        public void VerifyBannerExistenceChecked(Times times)
        {
            bannerRepository.Verify(b => b.AnyAsync(It.IsAny<Expression<Func<Banner, bool>>>()), times);
        }

        public void VerifyBannerSaved(Times times)
        {
            bannerRepository.Verify(x => x.SaveAsync(It.IsAny<Banner>()), times);
        }

        public void VerifyBannerUpdated(Times times)
        {
            bannerRepository.Verify(x => x.UpdateAsync(It.IsAny<Banner>()), times);
        }

        public void VerifyBannerRetrieved(int bannerId, Times times)
        {
            bannerRepository.Verify(x => x.GetByIdAsync(bannerId), times);
        }

        public void VerifyBannerDeleted(Banner banner, Times once)
        {
            bannerRepository.Verify(x => x.DeleteAsync(banner), once);
        }

        public void VerifyBannerPageListRetrieved(int page, int pageSize,
            string? sortBy, bool descending, Times times)
        {
            bannerRepository.Verify(x => x.GetPageList(page, pageSize, sortBy, descending, CancellationToken.None), times);
        }
    }
}
