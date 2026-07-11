using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Stabilization rules for shared persistence boundaries after BuildingBlocks extraction.
/// </summary>
public class PersistenceStabilizationTests
{
    [Fact]
    public void BuildingBlocks_Persistence_should_not_reference_module_Domain_or_Application()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Persistence.Repositories.BaseRepository<>).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
            .And()
            .NotHaveDependencyOn("Accounting.Application")
            .And()
            .NotHaveDependencyOn("Inventory.Domain")
            .And()
            .NotHaveDependencyOn("Inventory.Application")
            .And()
            .NotHaveDependencyOn("Identity.Domain")
            .And()
            .NotHaveDependencyOn("Identity.Application")
            .And()
            .NotHaveDependencyOn("ERP.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void ERP_Persistence_should_not_contain_generic_repository_query_helpers()
    {
        var assembly = typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly;

        var helperNames = new[]
        {
            "BaseEntityRepositoryQueries",
            "BaseSettingRepositoryQueries",
            "BaseTreeSettingRepositoryQueries"
        };

        foreach (var name in helperNames)
        {
            var types = Types.InAssembly(assembly)
                .That()
                .HaveName(name)
                .GetTypes();

            types.Should().BeEmpty($"{name} belongs in ERP.BuildingBlocks.Persistence");
        }
    }

    [Fact]
    public void Accounting_Infrastructure_should_not_reference_ERP_Persistence()
    {
        var result = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Infrastructure_should_not_reference_ERP_Persistence()
    {
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Infrastructure_should_not_reference_ApplicationDbContext()
    {
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Infrastructure.DBConfiguration.DbContext.ApplicationDbContext")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Module_Infrastructure_except_Identity_should_not_reference_ApplicationDbContext_type()
    {
        var accountingResult = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Infrastructure.DBConfiguration.DbContext.ApplicationDbContext")
            .GetResult();

        accountingResult.IsSuccessful.Should().BeTrue(FormatViolationMessage(accountingResult));

        var inventoryResult = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Infrastructure.DBConfiguration.DbContext.ApplicationDbContext")
            .GetResult();

        inventoryResult.IsSuccessful.Should().BeTrue(FormatViolationMessage(inventoryResult));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Persistence stabilization rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
