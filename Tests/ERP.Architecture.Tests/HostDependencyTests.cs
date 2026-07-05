using ERP.Host.Dev;
using ERP.Host.Security;
using FluentAssertions;
using Identity.Api.Controllers;
using Inventory.Api.Controllers;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 2 — composition root (ERP.Host) dependency and security conventions.
/// </summary>
public class HostDependencyTests
{
    [Fact]
    public void Host_should_not_reference_Identity_Domain()
    {
        var result = Types.InAssembly(typeof(DbWorkController).Assembly)
            .Should()
            .NotHaveDependencyOn("Identity.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void DbWork_controller_should_be_marked_development_only()
    {
        var attributes = typeof(DbWorkController).GetCustomAttributes(typeof(DevelopmentOnlyAttribute), inherit: true);
        attributes.Should().NotBeEmpty("seed tooling must not be exposed outside Development");
    }

    [Fact]
    public void Auth_controller_should_allow_anonymous_access()
    {
        typeof(AuthController)
            .GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute), inherit: true)
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public void Inventory_transfer_controller_should_require_authorization()
    {
        typeof(Inventory.Api.Controllers.Transactions.InventoryTransferController)
            .GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), inherit: true)
            .Should()
            .NotBeEmpty();
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Host dependency rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
