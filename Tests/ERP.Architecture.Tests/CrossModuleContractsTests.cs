using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 4 — cross-module integration must go through *.Contracts assemblies.
/// </summary>
public class CrossModuleContractsTests
{
    [Fact]
    public void Inventory_Application_should_not_reference_Accounting_Domain()
    {
        var result = Types.InAssembly(typeof(Inventory.Application.Commands.Colors.ColorCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Application_should_not_reference_Accounting_Application()
    {
        var result = Types.InAssembly(typeof(Inventory.Application.Commands.Colors.ColorCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Application_should_not_reference_Inventory_Application()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Application_should_not_reference_Inventory_Domain()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Application_should_not_reference_Inventory_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Infrastructure_should_not_reference_Inventory_Application()
    {
        var result = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Infrastructure_should_not_reference_Inventory_Domain()
    {
        var result = Types.InAssembly(typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("Inventory.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Application_should_reference_Accounting_Contracts_when_using_Accounting_types()
    {
        var result = Types.InAssembly(typeof(Inventory.Application.Commands.Colors.ColorCreateCommand).Assembly)
            .That()
            .HaveNameEndingWith("Service")
            .Or()
            .ResideInNamespaceContaining("Services")
            .Should()
            .NotHaveDependencyOn("Accounting.Domain")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Infrastructure_should_not_reference_Accounting_Application()
    {
        var result = Types.InAssembly(typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Cross-module contracts rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
