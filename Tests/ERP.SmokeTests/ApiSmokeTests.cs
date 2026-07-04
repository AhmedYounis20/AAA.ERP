using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ERP.SmokeTests;

/// <summary>
/// Phase 0 smoke tests — verifies routing, auth, and OpenAPI without a live SQL Server.
/// See docs/architecture/SMOKE_TEST_CHECKLIST.md for manual CRUD verification.
/// </summary>
public class ApiSmokeTests : IClassFixture<ErpWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(ErpWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory]
    [InlineData("/swagger/v1/swagger.json")]
    public async Task OpenApi_document_is_available(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"openapi\"");
    }

    [Theory]
    [InlineData("/api/Currencies")]
    [InlineData("/api/Products")]
    [InlineData("/api/Colors")]
    [InlineData("/api/Taxes")]
    [InlineData("/api/StockBalances")]
    [InlineData("/api/ChartOfAccounts")]
    public async Task Protected_endpoints_require_authentication(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Auth_login_endpoint_is_reachable()
    {
        var response = await _client.PostAsync(
            "/api/Auth/login",
            new StringContent("{}", System.Text.Encoding.UTF8, "application/json"));

        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.NotFound,
            HttpStatusCode.UnprocessableEntity);
    }
}
