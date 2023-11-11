using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
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
            var banner = new Banner
            {
                Active = true,
                Name = "Banner Test",
                Code = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now
            };

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(banner, CancellationToken.None);

            // Act
            var result = await service.CreateAsync(banner, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal("The start date cannot be greater than the end date.", result.Error);
        }

        [Fact]
        public async Task Create_ShouldBe_Success()
        {
            var banner = new Banner
            {
                Active = true,
                Name = "Banner Test",
                Code = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            _bannerRepositoryMock.Setup(x => x.SaveAsync(banner, CancellationToken.None)).Verifiable();

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(banner, CancellationToken.None);

            var result = await service.CreateAsync(banner, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Update_StartDateGreaterThanEndDate_ShouldBe_Failure()
        {
            var banner = new Banner
            {
                Active = true,
                Name = "Banner Test",
                Code = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now
            };

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.UpdateAsync(banner, CancellationToken.None);

            // Act
            var result = await service.CreateAsync(banner, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal("The start date cannot be greater than the end date.", result.Error);
        }

        [Fact]
        public async Task Update_ShouldBe_Success()
        {
            var banner = new Banner
            {
                Active = true,
                Name = "Banner Test",
                Code = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            _bannerRepositoryMock.Setup(x => x.SaveAsync(banner, CancellationToken.None)).Verifiable();

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.CreateAsync(banner, CancellationToken.None);

            var result = await service.UpdateAsync(banner, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_BannerNotFound_ShouldBe_Failure()
        {
            var banner = new Banner
            {
                Id = 1,
                Active = true,
                Name = "Banner Test",
                Code = Guid.NewGuid(),
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now
            };

            _bannerRepositoryMock.Setup(b => b.GetByIdAsync(1, CancellationToken.None)).Returns(Task.FromResult<Banner>(null));

            var service = _autoMocker.CreateInstance<BannerService>();
            await service.DeleteAsync(banner.Id, CancellationToken.None);

            // Act
            var result = await service.DeleteAsync(banner.Id, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            Assert.Equal($"Banner not found {banner.Id}.", result.Error);
        }

        //[Fact] arrumar teste
        //public async Task Delete_BannerNotFound_ShouldBe_Success()
        //{
        //    var banner = new Banner
        //    {
        //        Id = 1,
        //        Active = true,
        //        Name = "Banner Test",
        //        Code = Guid.NewGuid(),
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now.AddDays(7)
        //    };

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
