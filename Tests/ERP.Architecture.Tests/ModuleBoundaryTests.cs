using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Module boundary rules for the modular monolith migration.
/// </summary>
public class ModuleBoundaryTests
{
    [Fact(Skip = "Enable after Phase 1 — see docs/architecture/MODULAR_MONOLITH.md §10.3")]
    public void Inventory_services_should_not_reference_Account_domain_entities()
    {
        var result = Types.InAssembly(typeof(ERP.Infrastracture.Services.Inventory.ProductService).Assembly)
            .That()
            .ResideInNamespace("ERP.Infrastracture.Services.Inventory")
            .Should()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Module boundary violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
