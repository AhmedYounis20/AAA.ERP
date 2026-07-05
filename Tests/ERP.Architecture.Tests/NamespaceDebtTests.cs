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
    public void Platform_Caching_Application_should_not_use_legacy_ERP_Application_Services_Caching_namespace()
    {
        var result = Types.InAssembly(typeof(Platform.Caching.Application.ICacheService).Assembly)
            .Should()
            .NotResideInNamespace("ERP.Application.Services.Caching")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Application.Services.Caching.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Platform_Audit_Application_should_not_use_legacy_ERP_Application_Services_Audit_namespace()
    {
        var result = Types.InAssembly(typeof(Platform.Audit.Application.IAuditService).Assembly)
            .Should()
            .NotResideInNamespace("ERP.Application.Services.Audit")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Application.Services.Audit.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Persistence_ITransactionCoordinator_should_not_use_legacy_ERP_Application_Repositories_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Persistence.Repositories.ITransactionCoordinator).Assembly)
            .That()
            .HaveName("ITransactionCoordinator")
            .Should()
            .NotResideInNamespace("ERP.Application.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Inventory_Domain_lookup_DTOs_should_not_use_legacy_ERP_Domain_OutputDtos_Lookups_namespace()
    {
        var result = Types.InAssembly(typeof(Inventory.Domain.OutputDtos.Lookups.ColorLookupDto).Assembly)
            .That()
            .ResideInNamespaceContaining("Lookups")
            .And()
            .HaveNameEndingWith("LookupDto")
            .Should()
            .NotResideInNamespace("ERP.Domain.OutputDtos.Lookups")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_lookup_DTOs_should_not_use_legacy_ERP_Domain_OutputDtos_Lookups_namespace()
    {
        var result = Types.InAssembly(typeof(Accounting.Domain.OutputDtos.Lookups.CurrencyLookupDto).Assembly)
            .That()
            .ResideInNamespaceContaining("Lookups")
            .And()
            .HaveNameEndingWith("LookupDto")
            .Should()
            .NotResideInNamespace("ERP.Domain.OutputDtos.Lookups")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Identity_Application_should_not_use_legacy_ERP_Application_Services_Identity_namespace()
    {
        var result = Types.InAssembly(typeof(Identity.Application.IAuthenticationService).Assembly)
            .Should()
            .NotResideInNamespace("ERP.Application.Services.Identity")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Application.Services.Identity.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Persistence_ports_should_not_use_legacy_ERP_Application_Data_or_Repositories_namespace()
    {
        var assembly = typeof(ERP.BuildingBlocks.Persistence.Data.IApplicationDbContext).Assembly;

        var dataResult = Types.InAssembly(assembly)
            .That()
            .HaveName(nameof(ERP.BuildingBlocks.Persistence.Data.IApplicationDbContext))
            .Or()
            .HaveName(nameof(ERP.BuildingBlocks.Persistence.Data.ModuleModelConfigurationRegistry))
            .Should()
            .NotResideInNamespace("ERP.Application.Data")
            .GetResult();

        dataResult.IsSuccessful.Should().BeTrue(
            $"Legacy Data namespace still present: {string.Join(", ", dataResult.FailingTypes?.Select(t => t.FullName) ?? [])}");

        var uowResult = Types.InAssembly(assembly)
            .That()
            .HaveName("IUnitOfWork")
            .Should()
            .NotResideInNamespace("ERP.Application.Repositories")
            .GetResult();

        uowResult.IsSuccessful.Should().BeTrue(
            $"Legacy Repositories namespace still present: {string.Join(", ", uowResult.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Application_base_repositories_should_not_use_legacy_ERP_Application_Repositories_BaseRepositories_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly)
            .That()
            .ResideInNamespaceContaining("BaseRepositories")
            .Should()
            .NotResideInNamespace("ERP.Application.Repositories.BaseRepositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Inventory_Application_repositories_should_use_Inventory_Application_Repositories_namespace()
    {
        var result = Types.InAssembly(typeof(Inventory.Application.Repositories.IColorRepository).Assembly)
            .That()
            .HaveNameStartingWith("I")
            .And()
            .HaveNameEndingWith("Repository")
            .Should()
            .ResideInNamespace("Inventory.Application.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Unexpected namespace: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Application_repositories_should_use_Accounting_Application_Repositories_namespace()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Repositories.IEntryRepository).Assembly)
            .Should()
            .NotResideInNamespaceStartingWith("ERP.Application.Repositories")
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
