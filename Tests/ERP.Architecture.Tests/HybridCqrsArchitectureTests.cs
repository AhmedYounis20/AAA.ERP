using Accounting.Api.Controllers.SubLedger;
using FluentAssertions;
using Inventory.Api.Controllers.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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

    private static readonly string[] LegacyCrudBaseNames =
    [
        "BaseController`3",
        "BaseSettingController`3",
        "BaseTreeSettingController`3",
        "BaseTreeController`3"
    ];

    private static readonly Lazy<Type[]> HybridCqrsControllers = new(DiscoverHybridCqrsControllers);

    private static readonly Lazy<Type[]> ReadSideContracts = new(DiscoverReadSideContracts);

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
        HybridCqrsControllers.Value.Should().HaveCountGreaterThanOrEqualTo(35,
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
    public void Hybrid_controllers_should_not_inherit_legacy_crud_base_classes()
    {
        foreach (var controller in HybridCqrsControllers.Value)
        {
            for (var baseType = controller.BaseType; baseType is not null; baseType = baseType.BaseType)
            {
                LegacyCrudBaseNames.Should().NotContain(baseType.Name,
                    $"{controller.Name} must inherit ControllerBase directly, not legacy {baseType.Name}");
            }
        }
    }

    [Fact]
    public void Query_interfaces_should_not_extend_write_base_service()
    {
        ReadSideContracts.Value.Should().NotBeEmpty("expected query contracts in Application layers");

        foreach (var queryContract in ReadSideContracts.Value)
        {
            var hasWriteBase = queryContract.GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition().FullName?.Contains("IBaseService", StringComparison.Ordinal) == true);

            hasWriteBase.Should().BeFalse($"{queryContract.Name} must not extend IBaseService.");
        }
    }
}
