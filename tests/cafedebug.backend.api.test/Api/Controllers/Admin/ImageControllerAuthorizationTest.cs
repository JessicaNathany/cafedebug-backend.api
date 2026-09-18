using System.Reflection;
using System.Security.Claims;
using cafedebug_backend.api.Controllers.Admin;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace cafedebug.backend.api.test.Api.Controllers.Admin;

public class ImageControllerAuthorizationTest
{
    [Theory]
    [InlineData(nameof(ImageController.Upload))]
    [InlineData(nameof(ImageController.Delete))]
    public void ImageAdministrationAction_RequiresAuthenticatedUser(string actionName)
    {
        var action = typeof(ImageController).GetMethod(actionName, BindingFlags.Instance | BindingFlags.Public);

        action.Should().NotBeNull();
        action!.GetCustomAttributes<AuthorizeAttribute>().Should().ContainSingle();
    }

    [Fact]
    public async Task DefaultAuthorizationPolicy_RejectsAnonymousUsersAndAllowsAuthenticatedUsers()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAuthorization();
        await using var serviceProvider = services.BuildServiceProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")], "test"));

        var anonymousResult = await authorizationService.AuthorizeAsync(anonymousUser, null, policy);
        var authenticatedResult = await authorizationService.AuthorizeAsync(authenticatedUser, null, policy);

        anonymousResult.Succeeded.Should().BeFalse();
        authenticatedResult.Succeeded.Should().BeTrue();
    }
}
