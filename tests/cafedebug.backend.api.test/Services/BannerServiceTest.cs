using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Request;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services
{
    public class BannerServiceTest
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<IBannerRepository> _bannerRepositoryMock;
        private readonly Mock<IStringLocalizer> _localizerMock;
        private readonly Mock<ILogger<BannerService>> _loggerMock;

        public BannerServiceTest()
        {
            _autoMocker = new AutoMocker();

            _bannerRepositoryMock = new Mock<IBannerRepository>();
            _localizerMock = new Mock<IStringLocalizer>();
        }

        [Fact]
        public async Task Create_BannerIsNull_ShouldBe_Failure()
        {
            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(null, CancellationToken.None);

            //Act
            var result = await service.CreateAsync(null, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal("Banner cannot be null.", result.Error);
        }

        [Fact]
        public async Task Create_StartDateGreaterThanEndDate_ShouldBe_Failure()
        {
            var bannerRequest = new BannerRequest
            {
                Id = 1,  
                Name = "Banner Café Youtube",
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(7),
                Url = "http://teste.com/image",
                UrlImage = "http://teste.com/image",
                Active = true,
            };

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(bannerRequest, CancellationToken.None);

            // Act
            var result = await service.CreateAsync(bannerRequest, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal("The start date cannot be greater than the end date.", result.Error);
        }

        [Fact]
        public async Task Create_ShouldBe_Success()
        {
            var bannerRequest = new BannerRequest
            {
                Id = 1,
                Name = "Banner Café Youtube",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Url = "http://teste.com/image",
                UrlImage = "http://teste.com/image",
                Active = true,
            };

            _bannerRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Banner>(), CancellationToken.None)).Verifiable();

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(bannerRequest, CancellationToken.None);

            var banner = new Banner(
                bannerRequest.Name,
                bannerRequest.UrlImage, 
                bannerRequest.Url, 
                bannerRequest.StartDate,
                bannerRequest.EndDate,
                bannerRequest.Active);

            var result = await service.CreateAsync(bannerRequest, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Create_ShouldBe_ReturnException_BannerAlreadySameName()
        {
            var bannerRequest = new BannerRequest
            {
                Id = 1,
                Name = "Banner Café Youtube",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Url = "http://teste.com/image",
                UrlImage = "http://teste.com/image",
                Active = true,
            };

            var bannerExist = new Banner(
               bannerRequest.Name,
               "http://teste.com/image/youtube2",
               "http://teste.com/2",
               DateTime.Now.AddDays(-30),
               DateTime.Now.AddDays(-5),
               bannerRequest.Active);

            _bannerRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Banner>(), CancellationToken.None)).Verifiable();
            
            _bannerRepositoryMock.Setup(x => x.GetByNameAsync("Banner Café Youtube", CancellationToken.None))
                .Returns(Task.FromResult(bannerExist));

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(bannerRequest, CancellationToken.None);

            var result = await service.CreateAsync(bannerRequest, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal($"Banner already exists {bannerRequest.Name}.", result.Error);
        }

        //[Fact]
        //public async Task Update_StartDateGreaterThanEndDate_ShouldBe_Failure()
        //{
        //    var banner = new BannerRequest(
        //       Guid.NewGuid(),
        //       "Banner test",
        //       "http://teste.com/image",
        //       "http://teste.com/image",
        //       DateTime.Now.AddDays(2),
        //       DateTime.Now, true);

        //    var service = _autoMocker.CreateInstance<BannerService>();
        //    await service.UpdateAsync(banner, CancellationToken.None);

        //    // Act
        //    var result = await service.CreateAsync(banner, CancellationToken.None);

        //    // Assert
        //    Assert.False(result.IsSuccess);
        //    Assert.NotNull(result.Error);
        //    Assert.Equal("The start date cannot be greater than the end date.", result.Error);
        //}

        //[Fact]
        //public async Task Update_ShouldBe_Success()
        //{
        //    var banner = new BannerRequest(
        //       Guid.NewGuid(),
        //       "Banner test",
        //       "http://teste.com/image",
        //       "http://teste.com/image",
        //       DateTime.Now,
        //       DateTime.Now.AddDays(7), true);

        //    _bannerRepositoryMock.Setup(x => x.SaveAsync(banner, CancellationToken.None)).Verifiable();

        //    var service = _autoMocker.CreateInstance<BannerService>();
        //    await service.CreateAsync(banner, CancellationToken.None);

        //    var result = await service.UpdateAsync(banner, CancellationToken.None);

        //    Assert.True(result.IsSuccess);
        //}

        //[Fact]
        //public async Task Delete_BannerNotFound_ShouldBe_Failure()
        //{
        //    var banner = new BannerRequest(
        //        Guid.NewGuid(),
        //        "Banner test",
        //        "http://teste.com/image",
        //        "http://teste.com/image",
        //        DateTime.Now,
        //        DateTime.Now.AddDays(7), true);

        //    _bannerRepositoryMock.Setup(b => b.GetByIdAsync(1, CancellationToken.None)).Returns(Task.FromResult<BannerRequest>(null));

        //    var service = _autoMocker.CreateInstance<BannerService>();
        //    await service.DeleteAsync(banner.Id, CancellationToken.None);

        //    // Act
        //    var result = await service.DeleteAsync(banner.Id, CancellationToken.None);

        //    // Assert
        //    Assert.False(result.IsSuccess);
        //    Assert.NotNull(result.Error);
        //    Assert.Equal($"Banner not found {banner.Id}.", result.Error);
        //}

        //[Fact]
        //public async Task Delete_BannerNotFound_ShouldBe_Success()
        //{
        //    var banner = new BannerRequest(
        //        Guid.NewGuid(),
        //        "Banner test",
        //        "http://teste.com/image",
        //        "http://teste.com/image",
        //        DateTime.Now,
        //        DateTime.Now.AddDays(7), true);

        //    _bannerRepositoryMock.Setup(b => b.GetByIdAsync(1, CancellationToken.None)).ReturnsAsync(banner);

        //    var looggerMock = Mock.Of<ILogger<BannerService>>();
        //    var userService = new BannerService(_bannerRepositoryMock.Object, looggerMock);

        //    var service = _autoMocker.CreateInstance<BannerService>();
        //    await service.DeleteAsync(banner.Id, CancellationToken.None);

        //    // Act
        //    var result = await service.DeleteAsync(banner.Id, CancellationToken.None);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //}
    }
    
}
