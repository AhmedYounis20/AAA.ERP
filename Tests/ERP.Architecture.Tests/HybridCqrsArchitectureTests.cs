using Accounting.Api.Controllers.SubLedger;
using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Commands.Roles;
using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Models.Entities.CostCenters;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Application.Services.Queries;
using FluentAssertions;
using Inventory.Api.Controllers.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Architecture.Tests;

/// <summary>
/// Hybrid CQRS rules for Accounting and Inventory API controllers (ADR-012, Sprint 11 strict mode).
/// </summary>
public class HybridCqrsArchitectureTests
{
    private static readonly string[] ExcludedControllers =
    [
        "AuthController",
        "AuditLogsController",
        "DbWorkController"
    ];

    private static readonly Lazy<Type[]> HybridCqrsControllers = new(DiscoverHybridCqrsControllers);

    private static readonly Lazy<Type[]> ReadSideContracts = new(DiscoverReadSideContracts);

    private static readonly Type BaseSettingControllerDef =
        typeof(BaseSettingController<Role, RoleCreateCommand, RoleUpdateCommand>).GetGenericTypeDefinition();

    private static readonly Type BaseTreeSettingControllerDef =
        typeof(BaseTreeSettingController<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>).GetGenericTypeDefinition();

    private static readonly Type BaseSubLedgerControllerDef =
        typeof(BaseSubLedgerController<Tax, TaxCreateCommand, TaxUpdateCommand>).GetGenericTypeDefinition();

    private static readonly Type ISettingQueriesDef = typeof(ISettingQueries<>);
    private static readonly Type ITreeSettingQueriesDef = typeof(ITreeSettingQueries<>);
    private static readonly Type ISubLedgerQueriesDef = typeof(ISubLedgerQueries<,>);

    private enum RequiredBaseKind
    {
        None,
        Setting,
        Tree,
        SubLedger
    }

    private static Type[] DiscoverHybridCqrsControllers()
    {
        var excluded = new HashSet<string>(ExcludedControllers, StringComparer.Ordinal);

        return new[]
            {
                typeof(TaxesController).Assembly,
                typeof(ColorsController).Assembly
            }
            .SelectMany(assembly => assembly.GetExportedTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && type.Name.EndsWith("Controller", StringComparison.Ordinal)
                           && typeof(ControllerBase).IsAssignableFrom(type)
                           && !excluded.Contains(type.Name))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToArray();
    }

    private static Type[] DiscoverReadSideContracts()
    {
        return new[]
            {
                typeof(Accounting.Application.Services.ITaxQueries).Assembly,
                typeof(Inventory.Application.Services.IColorQueries).Assembly
            }
            .SelectMany(assembly => assembly.GetExportedTypes())
            .Where(type => type is { IsInterface: true, IsPublic: true }
                           && type.Namespace?.Contains(".Application.", StringComparison.Ordinal) == true
                           && type.Name.EndsWith("Queries", StringComparison.Ordinal)
                           && !type.Name.Contains("BaseQuery", StringComparison.Ordinal))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToArray();
    }

    [Fact]
    public void All_accounting_and_inventory_controllers_are_enforced()
    {
        HybridCqrsControllers.Value.Should().HaveCountGreaterThanOrEqualTo(34,
            "discovery should include all migrated Accounting + Inventory controllers");
    }

    [Fact]
    public void Hybrid_controllers_should_not_inject_legacy_write_services()
    {
        foreach (var controller in HybridCqrsControllers.Value)
        {
            var writeServiceParams = controller.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Where(p => p.ParameterType.Name.EndsWith("Service", StringComparison.Ordinal)
                            && !p.ParameterType.Name.Contains("Queries", StringComparison.Ordinal)
                            && !p.ParameterType.Name.Contains("QueryService", StringComparison.Ordinal)
                            && p.ParameterType.Name != "IAuthenticationService")
                .Select(p => p.ParameterType.Name)
                .Distinct()
                .ToList();

            writeServiceParams.Should().BeEmpty(
                $"{controller.Name} must inject ISender + query services only, not write services. Found: {string.Join(", ", writeServiceParams)}");
        }
    }

    [Fact]
    public void Hybrid_controllers_should_inject_ISender_when_they_have_write_actions()
    {
        foreach (var controller in HybridCqrsControllers.Value)
        {
            var hasWriteAction = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Any(m => m.GetCustomAttributes<HttpPostAttribute>().Any()
                          || m.GetCustomAttributes<HttpPutAttribute>().Any()
                          || m.GetCustomAttributes<HttpPatchAttribute>().Any()
                          || m.GetCustomAttributes<HttpDeleteAttribute>().Any());

            if (!hasWriteAction)
                continue;

            var hasSender = controller.GetConstructors()
                .Any(c => c.GetParameters().Any(p => p.ParameterType.Name is "ISender" or "IMediator"));

            hasSender.Should().BeTrue($"{controller.Name} has write actions and must inject ISender.");
        }
    }

    [Fact]
    public void Standard_setting_and_tree_controllers_should_inherit_base_controller()
    {
        var subjectCount = 0;

        foreach (var controller in HybridCqrsControllers.Value)
        {
            var queriesParameter = GetAggregateQueriesParameter(controller);
            if (queriesParameter is null)
                continue;

            var requiredBase = GetRequiredBase(queriesParameter.ParameterType);
            if (requiredBase == RequiredBaseKind.None)
                continue;

            subjectCount++;

            var hasRequiredBase = requiredBase switch
            {
                RequiredBaseKind.SubLedger => InheritsBaseController(controller, BaseSubLedgerControllerDef),
                RequiredBaseKind.Tree => InheritsBaseController(controller, BaseTreeSettingControllerDef)
                                               || InheritsBaseController(controller, BaseSubLedgerControllerDef),
                RequiredBaseKind.Setting => InheritsBaseController(controller, BaseSettingControllerDef)
                                                  || InheritsBaseController(controller, BaseTreeSettingControllerDef)
                                                  || InheritsBaseController(controller, BaseSubLedgerControllerDef),
                _ => false
            };

            hasRequiredBase.Should().BeTrue(
                $"{controller.Name} injects {queriesParameter.ParameterType.Name} ({requiredBase} queries) " +
                $"and must inherit BaseSettingController, BaseTreeSettingController, or BaseSubLedgerController " +
                $"to expose standard CRUD/search/tree endpoints.");
        }

        subjectCount.Should().BeGreaterThanOrEqualTo(19,
            "expected at least 19 standard setting/tree controllers on base controllers (Sprint 12 rollout)");
    }

    [Fact]
    public void Query_interfaces_should_not_extend_write_base_service()
    {
        ReadSideContracts.Value.Should().NotBeEmpty("expected query contracts in Application layers");

        foreach (var queryContract in ReadSideContracts.Value)
        {
            queryContract.Name.Should().NotEndWith("Service",
                $"{queryContract.Name} is a query contract and must not be named as a write service.");
        }
    }

    private static ParameterInfo? GetAggregateQueriesParameter(Type controller)
    {
        return controller.GetConstructors()
            .SelectMany(c => c.GetParameters())
            .FirstOrDefault(p => p.ParameterType is { IsInterface: true, Name: { } name }
                               && name.EndsWith("Queries", StringComparison.Ordinal)
                               && name.StartsWith('I')
                               && p.ParameterType.Namespace?.Contains(".Application.", StringComparison.Ordinal) == true);
    }

    private static RequiredBaseKind GetRequiredBase(Type queriesType)
    {
        var interfaces = queriesType.GetInterfaces();

        if (interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ISubLedgerQueriesDef))
            return RequiredBaseKind.SubLedger;

        if (interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ITreeSettingQueriesDef))
            return RequiredBaseKind.Tree;

        if (interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == ISettingQueriesDef))
            return RequiredBaseKind.Setting;

        return RequiredBaseKind.None;
    }

    private static bool InheritsBaseController(Type controller, Type openBaseControllerDefinition)
    {
        for (var baseType = controller.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == openBaseControllerDefinition)
                return true;
        }

        return false;
    }
}
