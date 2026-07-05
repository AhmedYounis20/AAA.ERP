using Accounting.Api;
using FluentAssertions;
using Identity.Api;
using Inventory.Api;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Module API layer rules — Phase 0 of the module-owned API migration.
/// See <c>docs/architecture/adr/011-module-owned-api.md</c>.
/// </summary>
public class ModuleApiLayerTests
{
    [Fact]
    public void Module_Api_should_not_reference_own_Domain_except_Accounting_CRUD_generics()
    {
        var identityResult = Types.InAssembly(IdentityApiAssemblyMarker.Assembly)
            .Should()
            .NotHaveDependencyOn("Identity.Domain")
            .And()
            .NotHaveDependencyOn("Accounting.Domain")
            .And()
            .NotHaveDependencyOn("Inventory.Domain")
            .GetResult();

        identityResult.IsSuccessful.Should().BeTrue(FormatViolationMessage("Identity.Api → *.Domain", identityResult));
    }

    [Fact]
    public void Module_Api_should_not_reference_own_Infrastructure()
    {
        foreach (var assembly in new[] { IdentityApiAssemblyMarker.Assembly, AccountingApiAssemblyMarker.Assembly, InventoryApiAssemblyMarker.Assembly })
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Identity.Infrastructure")
                .And()
                .NotHaveDependencyOn("Accounting.Infrastructure")
                .And()
                .NotHaveDependencyOn("Inventory.Infrastructure")
                .And()
                .NotHaveDependencyOn("ERP.Persistence")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(FormatViolationMessage($"Module.Api → Infrastructure ({assembly.GetName().Name})", result));
        }
    }

    [Fact]
    public void BuildingBlocks_Api_should_not_reference_Persistence()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Api.Controllers.BaseController<,,>).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .And()
            .NotHaveDependencyOn("Accounting.Infrastructure")
            .And()
            .NotHaveDependencyOn("Inventory.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage("BuildingBlocks.Api → Persistence/Infrastructure", result));
    }

    [Fact]
    public void Accounting_Api_may_reference_Accounting_Domain_for_CRUD_generics_only()
    {
        var result = Types.InAssembly(AccountingApiAssemblyMarker.Assembly)
            .That()
            .DoNotHaveNameEndingWith("Controller")
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage("Accounting.Api non-controllers → Domain", result));
    }

    [Fact]
    public void Inventory_Api_may_reference_Inventory_Domain_for_CRUD_generics_only()
    {
        var result = Types.InAssembly(InventoryApiAssemblyMarker.Assembly)
            .That()
            .DoNotHaveNameEndingWith("Controller")
            .Should()
            .NotHaveDependencyOn("Inventory.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage("Inventory.Api non-controllers → Domain", result));
    }

    [Fact(Skip = "Enable when DbWork migrates or Host controllers folder is removed. Current count: 1.")]
    public void Host_should_not_contain_business_controllers()
    {
        var hostAssembly = typeof(ERP.Host.Controllers.Accounting.DbWorkController).Assembly;
        var businessControllers = hostAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(t))
            .Where(t => t.Namespace?.StartsWith("ERP.Host.Controllers", StringComparison.Ordinal) == true)
            .Where(t => t.Namespace != "ERP.Host.Controllers.BaseControllers")
            .ToList();

        businessControllers.Should().BeEmpty(
            $"Business controllers must live in {{Module}}.Api projects. Remaining in Host: {string.Join(", ", businessControllers.Select(t => t.Name))}");
    }

    [Fact]
    public void Host_business_controller_count_baseline_for_ratchet()
    {
        var hostAssembly = typeof(ERP.Host.Controllers.Accounting.DbWorkController).Assembly;
        var count = hostAssembly.GetTypes()
            .Count(t => t.IsClass && !t.IsAbstract && typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(t)
                && t.Namespace?.StartsWith("ERP.Host.Controllers", StringComparison.Ordinal) == true
                && t.Namespace != "ERP.Host.Controllers.BaseControllers");

        count.Should().Be(1, "ratchet baseline — DbWork only; Host becomes composition root in Phase 5");
    }

    [Fact]
    public void Accounting_Domain_should_not_contain_Dto_types()
    {
        var dtoTypes = typeof(Accounting.Domain.Entities.Tax).Assembly.GetTypes()
            .Where(t => t.IsClass && (t.Name.EndsWith("Dto", StringComparison.Ordinal) || t.Name.EndsWith("InputModel", StringComparison.Ordinal)))
            .Select(t => t.FullName)
            .ToList();

        dtoTypes.Should().BeEmpty($"DTOs belong in Accounting.Application.Dtos: {string.Join(", ", dtoTypes)}");
    }

    [Fact]
    public void Inventory_Domain_should_not_contain_Dto_types()
    {
        var dtoTypes = typeof(Inventory.Domain.Models.Entities.Products.Product).Assembly.GetTypes()
            .Where(t => t.IsClass && (t.Name.EndsWith("Dto", StringComparison.Ordinal) || t.Name.EndsWith("InputModel", StringComparison.Ordinal)))
            .Select(t => t.FullName)
            .ToList();

        dtoTypes.Should().BeEmpty($"DTOs belong in Inventory.Application.Dtos: {string.Join(", ", dtoTypes)}");
    }

    private static string FormatViolationMessage(string rule, TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"{rule} rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
