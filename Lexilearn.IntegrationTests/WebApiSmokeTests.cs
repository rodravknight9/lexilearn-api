using System.Net;
using System.Net.Http.Json;

namespace Lexilearn.IntegrationTests;

public class WebApiSmokeTests : IClassFixture<LexilearnWebApplicationFactory>
{
    private readonly HttpClient _client;

    public WebApiSmokeTests(LexilearnWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public void Factory_CreatesHttpClient()
    {
        Assert.NotNull(_client);
        Assert.NotNull(_client.BaseAddress);
    }

    [Fact]
    public async Task Register_WithEmptyPayload_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/Auth/Register", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
