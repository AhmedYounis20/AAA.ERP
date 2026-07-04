using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Module boundary rules for the modular monolith migration.
/// </summary>
public class ModuleBoundaryTests
{
    [Fact]
    public void Inventory_transaction_services_should_not_reference_Account_domain_entities()
    {
        var result = Types.InAssembly(typeof(ERP.Infrastracture.Services.Inventory.ProductService).Assembly)
            .That()
            .HaveNameEndingWith("TransactionService")
            .Or()
            .HaveName("InventoryTransferService")
            .Should()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Domain_should_not_reference_Inventory_entities()
    {
        var result = Types.InAssembly(typeof(Accounting.Domain.Entities.Tax).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Inventory")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact(Skip = "Enable after Product/Variant cost-center and tax decoupling (Phase 3)")]
    public void Inventory_catalog_services_should_not_reference_Account_domain_entities()
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
