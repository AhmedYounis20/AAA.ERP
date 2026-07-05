using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 4.1.3 — ensures legacy <c>ERP.Infrastracture</c> namespace typo is fully retired.
/// </summary>
public class NamespaceDebtTests
{
    [Fact]
    public void Persistence_assembly_should_not_use_legacy_Infrastracture_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .Should()
            .NotResideInNamespace("ERP.Infrastracture")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Infrastracture.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Solution_should_not_contain_legacy_Compined_type_names()
    {
        var assemblies = new[]
        {
            typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly,
            typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly,
            typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly,
            typeof(ERP.API.Controllers.Account.Entries.CombinedEntriesController).Assembly
        };

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveNameMatching(".*Compined.*")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                $"Legacy Compined name in {assembly.GetName().Name}: {string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }
    }
}
