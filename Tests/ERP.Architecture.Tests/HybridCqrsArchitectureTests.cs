using Accounting.Api.Controllers.SubLedger;
using FluentAssertions;
using Inventory.Api.Controllers.Catalog;
using Inventory.Api.Controllers.Transactions;
using System.Reflection;

namespace ERP.Architecture.Tests;

/// <summary>
/// Hybrid CQRS rules for pilot controllers (see HYBRID_CQRS_PLAN.md Sprint 1).
/// </summary>
public class HybridCqrsArchitectureTests
{
    private static readonly Type[] PilotControllers =
    [
        typeof(TaxesController),
        typeof(ColorsController),
        typeof(ImportTransactionsController),
        typeof(ExportTransactionsController),
        typeof(InventoryTransferController)
    ];

    [Fact]
    public void Pilot_controllers_should_not_inject_legacy_write_services()
    {
        foreach (var controller in PilotControllers)
        {
            var writeServiceParams = controller.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Where(p => p.ParameterType.Name.EndsWith("Service", StringComparison.Ordinal)
                            && !p.ParameterType.Name.Contains("ReadService", StringComparison.Ordinal)
                            && !p.ParameterType.Name.Contains("QueryService", StringComparison.Ordinal)
                            && p.ParameterType.Name != "IAuthenticationService")
                .Select(p => p.ParameterType.Name)
                .Distinct()
                .ToList();

            writeServiceParams.Should().BeEmpty(
                $"{controller.Name} must inject ISender + read services only, not write services. Found: {string.Join(", ", writeServiceParams)}");
        }
    }

    [Fact]
    public void Pilot_controllers_should_inject_ISender()
    {
        foreach (var controller in PilotControllers)
        {
            var hasSender = controller.GetConstructors()
                .Any(c => c.GetParameters().Any(p => p.ParameterType.Name is "ISender" or "IMediator"));

            hasSender.Should().BeTrue($"{controller.Name} must inject ISender for write actions.");
        }
    }

    [Fact]
    public void Read_service_interfaces_should_not_extend_write_base_service()
    {
        var readServiceInterfaces = new[]
        {
            typeof(Accounting.Application.Services.ITaxReadService),
            typeof(Inventory.Application.Services.IColorReadService)
        };

        foreach (var readService in readServiceInterfaces)
        {
            var hasWriteBase = readService.GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition().FullName?.Contains("IBaseService") == true);

            hasWriteBase.Should().BeFalse($"{readService.Name} must not extend IBaseService.");
        }
    }
}
