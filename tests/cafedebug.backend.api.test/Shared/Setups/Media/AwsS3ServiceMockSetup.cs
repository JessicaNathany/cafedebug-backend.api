using cafedebug_backend.domain.Media;
using cafedebug_backend.domain.Media.Services;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Media;

public class AwsS3ServiceMockSetup(Mock<IFileService> fileService)
{
    public void UploadImage()
    {
        fileService
            .Setup(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ImageFolder>()))
            .ReturnsAsync("https://example.com/image.jpg");
    }
    
    public void UploadImageFailed()
    {
        fileService
            .Setup(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ImageFolder>()))
            .ReturnsAsync(string.Empty);
    }

    public void DeleteImage()
    {
        fileService
            .Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
    }

    public void DeleteImageFailed()
    {
        fileService
            .Setup(x => x.DeleteImageAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
    }
}