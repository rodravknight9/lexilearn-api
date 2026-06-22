using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Lexilearn.IntegrationTests;

public class LexilearnWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Key"] = "integration-test-signing-key-32chars!",
                ["JwtSettings:Issuer"] = "LexilearnTest",
                ["JwtSettings:Audience"] = "LexilearnTest",
                ["LibreTranslateSettings:Host"] = "http://localhost",
                ["LibreTranslateSettings:Port"] = "5000"
            });
        });
    }
}
