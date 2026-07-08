using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentAssertions;
using Inventory.Domain.Models.Entities.Products;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 0 — domain layer purity and cross-module API boundary rules.
/// </summary>
public class DomainLayerPurityTests
{
    [Fact]
    public void Accounting_Domain_should_not_reference_BuildingBlocks_Application()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.BuildingBlocks.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Domain_should_not_reference_BuildingBlocks_Application()
    {
        var result = Types.InAssembly(typeof(Product).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.BuildingBlocks.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Inventory_Api_should_not_reference_Accounting_Application()
    {
        var result = Types.InAssembly(typeof(Inventory.Api.Controllers.Catalog.ColorsController).Assembly)
            .Should()
            .NotHaveDependencyOn("Accounting.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void IBaseQueryService_should_reside_in_BuildingBlocks_Application()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Application.Services.Queries.IBaseQueryService<,>).Assembly)
            .That()
            .HaveName("IBaseQueryService")
            .Should()
            .ResideInNamespace("ERP.BuildingBlocks.Application.Services.Queries")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Accounting_Application_should_not_define_IBaseQueryService()
    {
        var types = Types.InAssembly(typeof(Accounting.Application.Repositories.IEntryRepository).Assembly)
            .That()
            .HaveName("IBaseQueryService")
            .GetTypes();

        types.Should().BeEmpty("IBaseQueryService belongs in ERP.BuildingBlocks.Application");
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Architecture rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
