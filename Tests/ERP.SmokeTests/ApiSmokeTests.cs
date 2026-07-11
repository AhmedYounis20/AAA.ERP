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
    [InlineData("/swagger/accounting/swagger.json")]
    [InlineData("/swagger/inventory/swagger.json")]
    [InlineData("/swagger/identity/swagger.json")]
    public async Task OpenApi_document_is_available(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("\"openapi\"");
    }

    [Theory]
    [InlineData("/api/Currencies")]
    [InlineData("/api/CompinedEntries")]
    [InlineData("/api/PackingUnits")]
    [InlineData("/api/Taxes")]
    [InlineData("/api/StockBalances")]
    [InlineData("/api/ChartOfAccounts")]
    [InlineData("/api/InventoryTransfer")]
    public async Task Protected_endpoints_require_authentication(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Responses_include_correlation_id_header()
    {
        var response = await _client.GetAsync("/api/Products");

        response.Headers.Should().ContainKey("X-Correlation-Id");
        response.Headers.GetValues("X-Correlation-Id").First().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Correlation_id_is_echoed_when_sent_by_client()
    {
        var expected = Guid.NewGuid().ToString("D");
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/Products");
        request.Headers.Add("X-Correlation-Id", expected);

        var response = await _client.SendAsync(request);

        response.Headers.GetValues("X-Correlation-Id").First().Should().Be(expected);
    }

    [Fact]
    public async Task Module_swagger_documents_filter_endpoints_by_module()
    {
        var accounting = await _client.GetStringAsync("/swagger/accounting/swagger.json");
        var inventory = await _client.GetStringAsync("/swagger/inventory/swagger.json");

        accounting.Should().Contain("/api/Currencies");
        accounting.Should().NotContain("/api/Products");

        inventory.Should().Contain("/api/Products");
        inventory.Should().NotContain("/api/Currencies");
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

/// <summary>Production-environment smoke checks (Phase 3).</summary>
public class ProductionApiSmokeTests : IClassFixture<ErpProductionWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductionApiSmokeTests(ErpProductionWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("/api/Dbwork/export/account")]
    [InlineData("/api/Dbwork/import/account")]
    public async Task Dbwork_endpoints_return_not_found_outside_development(string path)
    {
        var response = await _client.GetAsync(path);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
