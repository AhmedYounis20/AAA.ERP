using Accounting.Domain.Models.Entities.Account.SubLeadgers;
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
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .That()
            .HaveNameEndingWith("TransactionService")
            .Or()
            .HaveName("InventoryTransferService")
            .Should()
            .NotHaveDependencyOn("Accounting.Domain.Models.Entities")
            .And()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Domain_should_not_reference_Inventory_entities()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Domain.Models.Entities")
            .And()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Inventory")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Domain_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .And()
            .NotHaveDependencyOn("Accounting.Infrastructure")
            .And()
            .NotHaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Application_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .And()
            .NotHaveDependencyOn("Accounting.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Infrastructure_should_not_reference_Inventory_entities()
    {
        var result = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .That()
            .DoNotHaveName("FinancialPeriodUsageQueries")
            .Should()
            .NotHaveDependencyOn("Inventory.Domain.Models.Entities")
            .And()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Inventory")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Persistence_kernel_should_not_contain_Accounting_horizontal_services()
    {
        var horizontalTypes = Types.InAssembly(typeof(ERP.Infrastructure.DBConfiguration.DbContext.ApplicationDbContext).Assembly)
            .That()
            .ResideInNamespace("ERP.Infrastructure.Services.Account")
            .Or()
            .ResideInNamespace("ERP.Infrastructure.Repositories.Account")
            .GetTypes();

        horizontalTypes.Should().BeEmpty();
    }

    [Fact]
    public void Accounting_Infrastructure_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Host")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Application_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Inventory.Application.Commands.Colors.ColorCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .And()
            .NotHaveDependencyOn("Inventory.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Infrastructure_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Host")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_catalog_services_should_not_reference_Account_domain_entities()
    {
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .That()
            .ResideInNamespace("Inventory.Infrastructure.Services")
            .Should()
            .NotHaveDependencyOn("Accounting.Domain.Models.Entities")
            .And()
            .NotHaveDependencyOn("ERP.Domain.Models.Entities.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Domain_should_not_reference_Accounting_Domain()
    {
        var result = Types.InAssembly(typeof(Inventory.Domain.Models.Entities.Products.Product).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
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
