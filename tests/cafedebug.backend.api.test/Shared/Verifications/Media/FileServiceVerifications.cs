using cafedebug_backend.domain.Media;
using cafedebug_backend.domain.Media.Services;
using Moq;

namespace cafedebug.backend.api.test.Shared.Verifications.Media;

public class FileServiceVerifications(Mock<IFileService> fileService)
{
    public void VerifyImageUpload(Times times)
    {
        fileService.Verify(x => x.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ImageFolder>()), times);
    }
    
    public void VerifyImageDelete(Times times)
    {
        fileService.Verify(x => x.DeleteImageAsync(It.IsAny<string>()), times);
    }
}