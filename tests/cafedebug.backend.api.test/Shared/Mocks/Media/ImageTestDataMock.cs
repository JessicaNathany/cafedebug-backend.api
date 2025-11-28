using AutoFixture;
using cafedebug.backend.application.Media.DTOs.Requests;

namespace cafedebug.backend.api.test.Shared.Mocks.Media;

public class ImageTestDataMock(IFixture fixture)
{
    public UploadImageRequest CreateUploadImageRequest()
    {
        var builder = fixture.Build<UploadImageRequest>();
        return builder.Create();
    }

    public DeleteImageRequest CreateDeleteImageRequest()
    {
        var builder = fixture.Build<DeleteImageRequest>();
       return builder.Create();
    }
}