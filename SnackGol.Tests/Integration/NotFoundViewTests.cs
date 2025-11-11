using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SnackGol.Tests.Integration;

public class NotFoundViewTests : IClassFixture<WebApplicationFactory<MSSnackGolFrontend.Controllers.HomeController>>
{
    private readonly WebApplicationFactory<MSSnackGolFrontend.Controllers.HomeController> _factory;

    public NotFoundViewTests(WebApplicationFactory<MSSnackGolFrontend.Controllers.HomeController> factory)
    {
        _factory = factory;
    }

    // Verifica que al solicitar una ruta inexistente en el Frontend,
    // se devuelva la vista de NotFound con estado HTTP 404.
    [Fact]
    public async Task MissingPage_ReturnsNotFoundView_With404Status()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/this-page-does-not-exist-xyz");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/html");

        var html = await response.Content.ReadAsStringAsync();
        // FluentAssertions no provee OrContain; validamos que exista al menos uno de los textos esperados
        (html.Contains("Lo sentimos") || html.Contains("404")).Should().BeTrue();
    }
}
