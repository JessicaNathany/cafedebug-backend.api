using cafedebug_backend.domain.Media.Services;
using cafedebug.backend.api.test.Shared;
using cafedebug.backend.api.test.Shared.Mocks.Media;
using cafedebug.backend.api.test.Shared.Setups.Media;
using cafedebug.backend.api.test.Shared.Verifications.Media;
using cafedebug.backend.application.Media.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Application.Media.Services;

[Collection("MediaTests")]
public class ImageServiceTest : BaseTest
{
    private readonly ImageService _imageService;

    private readonly ImageTestDataMock _imageTestDataMock;
    private readonly AwsS3ServiceMockSetup _awsS3ServiceMockSetup;
    private readonly FileServiceVerifications _fileServiceVerifications;

    public ImageServiceTest()
    {
        var fileServiceMock = new Mock<IFileService>();

        _imageTestDataMock = new ImageTestDataMock(Fixture);
        _awsS3ServiceMockSetup = new AwsS3ServiceMockSetup(fileServiceMock);
        _fileServiceVerifications = new FileServiceVerifications(fileServiceMock);

        _imageService = new ImageService(fileServiceMock.Object);
    }

    [Fact]
    public async Task UploadAsync_ReturnSuccessResult()
    {
        // Arrange
        var request = _imageTestDataMock.CreateUploadImageRequest();

        _awsS3ServiceMockSetup.UploadImage();

        // Act
        var result = await _imageService.UploadAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.ImageUrl.Should().NotBeNullOrWhiteSpace();

        _fileServiceVerifications.VerifyImageUpload(Times.Once());
    }

    [Fact]
    public async Task UploadAsync_WhenUploadFails_ReturnFailedResult()
    {
        // Arrange
        var request = _imageTestDataMock.CreateUploadImageRequest();

        _awsS3ServiceMockSetup.UploadImageFailed();

        // Act
        var result = await _imageService.UploadAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();

        _fileServiceVerifications.VerifyImageUpload(Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ReturnSuccessResult()
    {
        // Arrange
        var request = _imageTestDataMock.CreateDeleteImageRequest();

        _awsS3ServiceMockSetup.DeleteImage();

        // Act
        var result = await _imageService.DeleteAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        _fileServiceVerifications.VerifyImageDelete(Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_WhenDeleteFails_ReturnFailedResult()
    {
        // Arrange
        var request = _imageTestDataMock.CreateDeleteImageRequest();
        
        _awsS3ServiceMockSetup.DeleteImageFailed();
        
        // Act
        var result = await _imageService.DeleteAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}