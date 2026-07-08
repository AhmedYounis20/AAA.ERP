using Accounting.Domain.Models.Entities.SubLeadgers;
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
        var result = Types.InAssembly(typeof(Inventory.Application.Dtos.Lookups.ColorLookupDto).Assembly)
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
        var result = Types.InAssembly(typeof(Accounting.Application.Dtos.Lookups.CurrencyLookupDto).Assembly)
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
    public void Identity_Infrastructure_should_not_use_legacy_Domain_Account_Services_Identity_namespace()
    {
        var result = Types.InAssembly(typeof(Identity.Infrastructure.IdentityModuleExtensions).Assembly)
            .Should()
            .NotResideInNamespace("Domain.Account.Services.Identity")
            .And()
            .NotResideInNamespaceStartingWith("Domain.Account.Services.Identity.")
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
    public void BuildingBlocks_Application_base_services_should_not_use_legacy_ERP_Application_Services_BaseServices_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly)
            .That()
            .ResideInNamespaceContaining("BaseServices")
            .Should()
            .NotResideInNamespace("ERP.Application.Services.BaseServices")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void IBaseQueryService_should_not_reside_in_Accounting_Application_namespace()
    {
        var assembly = typeof(Accounting.Application.Repositories.IEntryRepository).Assembly;

        var queryResult = Types.InAssembly(assembly)
            .That()
            .HaveName("IBaseQueryService")
            .Should()
            .NotResideInNamespace("Accounting.Application.Services.Queries")
            .And()
            .NotResideInNamespaceStartingWith("Accounting.Application.Services.Queries.")
            .And()
            .NotResideInNamespace("ERP.Application.Services.Account")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Application.Services.Account.")
            .GetResult();

        queryResult.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", queryResult.FailingTypes?.Select(t => t.FullName) ?? [])}");
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
    public void Inventory_Domain_entities_should_not_use_legacy_ERP_Domain_Models_Entities_Inventory_namespace()
    {
        var result = Types.InAssembly(typeof(Inventory.Domain.Models.Entities.Colors.Color).Assembly)
            .That()
            .ResideInNamespace("Inventory.Domain.Models.Entities")
            .Or()
            .ResideInNamespaceStartingWith("Inventory.Domain.Models.Entities.")
            .Should()
            .NotResideInNamespaceStartingWith("ERP.Domain.Models.Entities.Inventory")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_entities_should_not_use_legacy_ERP_Domain_Models_Entities_Account_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .That()
            .ResideInNamespaceStartingWith("Accounting.Domain.Models.Entities")
            .Should()
            .NotResideInNamespaceStartingWith("ERP.Domain.Models.Entities.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Inventory_Domain_DTOs_should_not_use_legacy_ERP_Domain_Models_Dtos_Inventory_namespace()
    {
        var result = Types.InAssembly(typeof(Inventory.Domain.Models.Entities.Colors.Color).Assembly)
            .That()
            .ResideInNamespaceContaining("Dtos")
            .Should()
            .NotResideInNamespace("ERP.Domain.Models.Dtos.Inventory")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Domain.Models.Dtos.Inventory.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_commands_should_not_use_legacy_ERP_Domain_Commands_Account_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .Should()
            .NotResideInNamespace("ERP.Domain.Commands.Account")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Domain.Commands.Account.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Application_base_validators_should_not_use_legacy_ERP_Application_Validators_Account_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly)
            .That()
            .ResideInNamespaceContaining("Validators")
            .Should()
            .NotResideInNamespace("ERP.Application.Validators.Account")
            .And()
            .NotResideInNamespaceStartingWith("ERP.Application.Validators.Account.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Application_validators_should_not_use_legacy_ERP_Application_Validators_Account_namespace()
    {
        var result = Types.InAssembly(typeof(Accounting.Application.Repositories.IEntryRepository).Assembly)
            .That()
            .ResideInNamespaceContaining("Validators")
            .Should()
            .NotResideInNamespaceStartingWith("ERP.Application.Validators.Account")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Application_base_commands_should_not_use_legacy_Domain_Account_Commands_or_InputModels_namespace()
    {
        var assembly = typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly;

        var commandsResult = Types.InAssembly(assembly)
            .That()
            .ResideInNamespaceContaining("Commands")
            .Should()
            .NotResideInNamespaceStartingWith("Domain.Account.Commands")
            .GetResult();

        commandsResult.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", commandsResult.FailingTypes?.Select(t => t.FullName) ?? [])}");

        var inputModelsResult = Types.InAssembly(assembly)
            .That()
            .ResideInNamespaceContaining("BaseInputModels")
            .Should()
            .NotResideInNamespaceStartingWith("Domain.Account.InputModels")
            .GetResult();

        inputModelsResult.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", inputModelsResult.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_models_dtos_should_not_use_legacy_Domain_Account_Models_Dtos_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .That()
            .ResideInNamespaceContaining("Models.Dtos")
            .Should()
            .NotResideInNamespaceStartingWith("Domain.Account.Models.Dtos")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Identity_Application_auth_dtos_should_not_use_legacy_Domain_Account_Models_Dtos_namespace()
    {
        var result = Types.InAssembly(typeof(Identity.Application.IAuthenticationService).Assembly)
            .That()
            .ResideInNamespaceContaining("Models.Dtos")
            .Should()
            .NotResideInNamespace("Domain.Account.Models.Dtos")
            .And()
            .NotResideInNamespaceStartingWith("Domain.Account.Models.Dtos.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Persistence_kernel_should_not_use_legacy_Domain_Account_DBConfiguration_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.Infrastructure.DBConfiguration.DbContext.ApplicationDbContext).Assembly)
            .Should()
            .NotResideInNamespaceStartingWith("Domain.Account.DBConfiguration")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_utility_should_not_use_legacy_Domain_Account_Utility_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .That()
            .ResideInNamespaceContaining("Utility")
            .Should()
            .NotResideInNamespace("Domain.Account.Utility")
            .And()
            .NotResideInNamespaceStartingWith("Domain.Account.Utility.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_Application_base_dtos_should_not_use_legacy_AAA_ERP_OutputDtos_namespace()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly)
            .That()
            .ResideInNamespaceContaining("DTOs")
            .Should()
            .NotResideInNamespaceStartingWith("AAA.ERP.OutputDtos")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Accounting_Domain_output_dtos_should_not_use_legacy_AAA_ERP_OutputDtos_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .That()
            .ResideInNamespaceContaining("OutputDtos")
            .Should()
            .NotResideInNamespace("AAA.ERP.OutputDtos")
            .And()
            .NotResideInNamespaceStartingWith("AAA.ERP.OutputDtos.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Host_project_should_not_use_legacy_ERP_API_namespace()
    {
        var result = Types.InAssembly(typeof(Inventory.Api.Controllers.Catalog.ColorsController).Assembly)
            .Should()
            .NotResideInNamespace("ERP.API")
            .And()
            .NotResideInNamespaceStartingWith("ERP.API.")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Legacy namespace still present: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void BuildingBlocks_and_modules_should_not_declare_new_Shared_namespaces()
    {
        var assemblies = new[]
        {
            typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly,
            typeof(ERP.BuildingBlocks.Domain.Entities.BaseEntity).Assembly,
            typeof(Accounting.Application.Repositories.IEntryRepository).Assembly,
            typeof(Inventory.Application.Repositories.IColorRepository).Assembly,
            typeof(Identity.Application.IAuthenticationService).Assembly,
            typeof(Platform.Audit.Application.IAuditService).Assembly,
            typeof(Accounting.Api.Controllers.Entries.CombinedEntriesController).Assembly
        };

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotResideInNamespaceStartingWith("Shared.")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                $"Shared.* namespace still present in {assembly.GetName().Name}: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
        }
    }

    [Fact]
    public void Solution_should_not_contain_ComandValidators_typo_in_namespaces()
    {
        var assemblies = new[]
        {
            typeof(ERP.BuildingBlocks.Application.Repositories.BaseRepositories.IBaseRepository<>).Assembly,
            typeof(Accounting.Application.Repositories.IEntryRepository).Assembly,
            typeof(Inventory.Application.Repositories.IColorRepository).Assembly
        };

        foreach (var assembly in assemblies)
        {
            var result = Types.InAssembly(assembly)
                .Should()
                .NotResideInNamespaceContaining("ComandValidators")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                $"ComandValidators typo in {assembly.GetName().Name}: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
        }
    }

    [Fact]
    public void Accounting_Domain_should_not_contain_commands_namespace()
    {
        var result = Types.InAssembly(typeof(Tax).Assembly)
            .Should()
            .NotResideInNamespaceContaining("Commands")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"Domain commands should live in Application: {string.Join(", ", result.FailingTypes?.Select(t => t.FullName) ?? [])}");
    }

    [Fact]
    public void Solution_should_not_contain_legacy_Compined_type_names()
    {
        var assemblies = new[]
        {
            typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly,
            typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly,
            typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly,
            typeof(Accounting.Api.Controllers.Entries.CombinedEntriesController).Assembly
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
