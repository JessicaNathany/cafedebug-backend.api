using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using cafedebug_backend.domain.Media;
using cafedebug_backend.infrastructure.Storage;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace cafedebug.backend.api.test.Infrastructure.Storage;

public class AwsS3ServiceTest
{
    private static readonly StorageSettings AwsSettings = new()
    {
        Bucket = "cafedebug-images",
        BaseUrl = "https://cafedebug-images.s3.sa-east-1.amazonaws.com",
        Region = "sa-east-1"
    };

    [Fact]
    public async Task UploadImageAsync_WhenUploadSucceeds_ReturnsPublicUrlWithoutCannedAcl()
    {
        var s3Client = new Mock<IAmazonS3>();
        PutObjectRequest? uploadedRequest = null;

        s3Client
            .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<PutObjectRequest, CancellationToken>((request, _) => uploadedRequest = request)
            .ReturnsAsync(new PutObjectResponse { HttpStatusCode = HttpStatusCode.OK });

        var service = CreateService(s3Client.Object);

        var result = await service.UploadImageAsync("data:image/png;base64,aGVsbG8=", "cover.png", ImageFolder.Episodes);

        result.Should().Be("https://cafedebug-images.s3.sa-east-1.amazonaws.com/episodes/cover.png");
        uploadedRequest.Should().NotBeNull();
        uploadedRequest!.BucketName.Should().Be("cafedebug-images");
        uploadedRequest.Key.Should().Be("episodes/cover.png");
        uploadedRequest.CannedACL.Should().BeNull();
    }

    [Theory]
    [InlineData("https://cafedebug-images.s3.sa-east-1.amazonaws.com/episodes/cover.png", "episodes/cover.png")]
    [InlineData("https://s3.sa-east-1.amazonaws.com/cafedebug-images/banners/banner.png", "banners/banner.png")]
    [InlineData("https://cafedebug-images.s3.amazonaws.com/cafedebug-images/team-members/member.png", "team-members/member.png")]
    public async Task DeleteImageAsync_WhenUrlUsesCurrentOrLegacyS3Format_DeletesExpectedKey(string imageUrl, string expectedKey)
    {
        var s3Client = new Mock<IAmazonS3>();
        DeleteObjectRequest? deleteRequest = null;

        s3Client
            .Setup(client => client.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()))
            .Callback<DeleteObjectRequest, CancellationToken>((request, _) => deleteRequest = request)
            .ReturnsAsync(new DeleteObjectResponse { HttpStatusCode = HttpStatusCode.NoContent });

        var service = CreateService(s3Client.Object);

        var result = await service.DeleteImageAsync(imageUrl);

        result.Should().BeTrue();
        deleteRequest.Should().NotBeNull();
        deleteRequest!.BucketName.Should().Be("cafedebug-images");
        deleteRequest.Key.Should().Be(expectedKey);
    }

    [Fact]
    public async Task DeleteImageAsync_WhenUrlDoesNotBelongToConfiguredBucket_DoesNotDelete()
    {
        var s3Client = new Mock<IAmazonS3>();
        var service = CreateService(s3Client.Object);

        var result = await service.DeleteImageAsync("https://example.com/episodes/cover.png");

        result.Should().BeFalse();
        s3Client.Verify(client => client.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UploadImageAsync_WithCustomEndpoint_PreservesConfiguredLocalPublicUrl()
    {
        var s3Client = new Mock<IAmazonS3>();
        s3Client
            .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutObjectResponse { HttpStatusCode = HttpStatusCode.OK });
        var localSettings = new StorageSettings
        {
            Bucket = "cafedebug-images",
            ServiceUrl = "http://localhost:9000",
            BaseUrl = "http://localhost:9000/cafedebug-images",
            ForcePathStyle = true,
            UseHttp = true
        };
        var service = new AwsS3Service(s3Client.Object, localSettings, NullLogger<AwsS3Service>.Instance);

        var result = await service.UploadImageAsync("aGVsbG8=", "cover.png", ImageFolder.Banners);

        result.Should().Be("http://localhost:9000/cafedebug-images/banners/cover.png");
    }

    [Fact]
    public void Validate_WhenAwsRegionIsMissing_ReturnsFailure()
    {
        var result = new StorageSettingsValidator().Validate(null, new StorageSettings
        {
            Bucket = "cafedebug-images",
            BaseUrl = "https://cafedebug-images.s3.sa-east-1.amazonaws.com"
        });

        result.Failed.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "https://cafedebug-images.s3.sa-east-1.amazonaws.com", "sa-east-1")]
    [InlineData("cafedebug-images", "http://cafedebug-images.s3.sa-east-1.amazonaws.com", "sa-east-1")]
    public void Validate_WhenProductionSettingsAreInvalid_ReturnsFailure(string bucket, string baseUrl, string region)
    {
        var result = new StorageSettingsValidator().Validate(null, new StorageSettings
        {
            Bucket = bucket,
            BaseUrl = baseUrl,
            Region = region
        });

        result.Failed.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenLocalS3ConfigurationIsComplete_ReturnsSuccess()
    {
        var result = new StorageSettingsValidator().Validate(null, new StorageSettings
        {
            Bucket = "cafedebug-images",
            ServiceUrl = "http://localhost:9000",
            BaseUrl = "http://localhost:9000/cafedebug-images",
            ForcePathStyle = true,
            UseHttp = true
        });

        result.Succeeded.Should().BeTrue();
    }

    private static AwsS3Service CreateService(IAmazonS3 s3Client)
    {
        return new AwsS3Service(s3Client, AwsSettings, NullLogger<AwsS3Service>.Instance);
    }
}
