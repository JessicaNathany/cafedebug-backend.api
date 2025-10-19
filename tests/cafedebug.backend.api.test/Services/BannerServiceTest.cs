using cafedebug_backend.domain.Banners;
using cafedebug.backend.application.Service;
using cafedebug_backend.domain.Banners.Errors;
using cafedebug_backend.domain.Banners.Repositories;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug.backend.application.Accounts;
using cafedebug.backend.application.Accounts.Services;
using cafedebug.backend.application.Banners;
using cafedebug.backend.application.Banners.DTOs.Requests;
using cafedebug.backend.application.Banners.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace cafedebug.backend.api.test.Services;

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
        // TESTE será refatorado assim que o endpoint de Banner estiver pronto
        //var service = _autoMocker.CreateInstance<BannerService>();
        //await service.CreateAsync(null);

        ////Act
        //var result = await service.CreateAsync(null);

        //// Assert
        //Assert.False(result.IsSuccess);
        //Assert.NotNull(result.Error);
        //Assert.Equal("Banner cannot be null.", result.Error);
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

        var banner = new Banner(
            bannerRequest.Name,
            bannerRequest.UrlImage,
            bannerRequest.Url,
            bannerRequest.StartDate,
            bannerRequest.EndDate,
            bannerRequest.Active,
            bannerRequest.Ordem);

        _bannerRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Banner>())).Verifiable();

        var service = _autoMocker.CreateInstance<BannerService>();
        await service.CreateAsync(banner);

        var result = await service.CreateAsync(banner);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Create_ShouldBe_ReturnException_BannerAlreadySameName()
    {
        // TESTE será refatorado assim que o endpoint de Banner estiver pronto

        //var bannerRequest = new BannerRequest
        //{
        //    Id = 1,
        //    Name = "Banner Café Youtube",
        //    StartDate = DateTime.Now,
        //    EndDate = DateTime.Now.AddDays(7),
        //    Url = "http://teste.com/image",
        //    UrlImage = "http://teste.com/image",
        //    Active = true,
        //    Ordem = 1
        //};

        //var bannerExist = new Banner(
        //   bannerRequest.Name,
        //   "http://teste.com/image/youtube2",
        //   "http://teste.com/2",
        //   DateTime.Now.AddDays(-30),
        //   DateTime.Now.AddDays(-5),
        //   bannerRequest.Active,
        //   bannerRequest.Ordem = 1);

        //var looggerMock = Mock.Of<ILogger<BannerService>>();
        //var stringLocalizerMock = Mock.Of<IStringLocalizer<UserService>>();


        //_bannerRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Banner>()));
            
        //_bannerRepositoryMock.Setup(x => x.GetByNameAsync("Banner Café Youtube"))
        //    .Returns(Task.FromResult(bannerExist));

        //var service = new BannerService(_bannerRepositoryMock.Object, looggerMock);
        //await service.CreateAsync(It.IsAny<Banner>());

        //var result = await service.CreateAsync(bannerExist);

        //// Assert
        //Assert.False(result.IsSuccess);
        //Assert.NotNull(result.Error);
        //Assert.Equal($"Banner already exists {bannerRequest.Name}.", result.Error);
    }

    [Fact]
    public async Task Update_StartDateGreaterThanEndDate_ShouldBe_Failure()
    {
        var bannerRequest = new BannerRequest
        {
            Id = 1,
            Name = "Banner Café Youtube",
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now,
            Url = "http://teste.com/image",
            UrlImage = "http://teste.com/image",
            Active = true,
            Ordem = 1
        };

        var banner = new Banner(
            bannerRequest.Name,
            bannerRequest.UrlImage,
            bannerRequest.Url,
            bannerRequest.StartDate,
            bannerRequest.EndDate,
            bannerRequest.Active,
            bannerRequest.Ordem);

        var service = _autoMocker.CreateInstance<BannerService>();
        await service.UpdateAsync(banner);

        // Act
        var result = await service.UpdateAsync(banner);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task Update_ShouldBe_Success()
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
            Ordem = 1
        };

        var bannerExist = new Banner(
            bannerRequest.Name,
            bannerRequest.UrlImage,
            bannerRequest.Url,
            bannerRequest.StartDate,
            bannerRequest.EndDate,
            bannerRequest.Active,
            bannerRequest.Ordem);
        bannerExist.Id = 1;    

        var looggerMock = Mock.Of<ILogger<BannerService>>();
        var stringLocalizerMock = Mock.Of<IStringLocalizer<UserService>>();

        _bannerRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(bannerExist));
        _bannerRepositoryMock.Setup(x => x.SaveAsync(It.IsAny<Banner>())).Verifiable();

        //Act
        var service = new BannerService(_bannerRepositoryMock.Object, looggerMock);
        var result = await service.UpdateAsync(bannerExist);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    //[Fact]
    //public async Task Delete_BannerNotFound_ShouldBe_Failure()
    //{
    //    var bannerRequest = new BannerRequest
    //    {
    //        Id = 1,
    //        Name = "Banner Café Youtube",
    //        StartDate = DateTime.Now,
    //        EndDate = DateTime.Now.AddDays(7),
    //        Url = "http://teste.com/image",
    //        UrlImage = "http://teste.com/image",
    //        Active = true,
    //    };

    //    _bannerRepositoryMock.Setup(b => b.GetByIdAsync(1, CancellationToken.None)).Returns(Task.FromResult<Banner>(null));

    //    var service = _autoMocker.CreateInstance<BannerService>();
    //    await service.DeleteAsync(bannerRequest.Id, CancellationToken.None);

    //    // Act
    //    var result = await service.DeleteAsync(bannerRequest.Id, CancellationToken.None);

    //    // Assert
    //    Assert.False(result.IsSuccess);
    //    Assert.NotNull(result.Error);
    //    Assert.Equal($"Banner not found {bannerRequest.Id}.", result.Error);
    //}

    [Fact]
    public async Task Delete_BannerNotFound_ShouldBe_Success()
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
            Ordem = 1   
        };

        var banner = new Banner(
            bannerRequest.Name,
            bannerRequest.UrlImage,
            bannerRequest.Url,
            bannerRequest.StartDate,
            bannerRequest.EndDate,
            bannerRequest.Active, 
            bannerRequest.Ordem);

        _bannerRepositoryMock.Setup(b => b.GetByIdAsync(1)).ReturnsAsync(banner);

        var looggerMock = Mock.Of<ILogger<BannerService>>();
        var userService = new BannerService(_bannerRepositoryMock.Object, looggerMock);

        var service = _autoMocker.CreateInstance<BannerService>();
        await service.DeleteAsync(bannerRequest.Id);

        // Act
        var result = await service.DeleteAsync(bannerRequest.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(BannerError.NotFound(bannerRequest.Id).Message, result.Error.Message);
    }
}