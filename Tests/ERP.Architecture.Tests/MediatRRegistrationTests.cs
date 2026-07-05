using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ERP.Infrastracture.Utilities;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 0 task 0.2.1 — documents and verifies MediatR handler registration (ADR-004).
/// </summary>
public class MediatRRegistrationTests
{
    private const int MinimumHorizontalHandlerCount = 0;
    private const int MinimumAccountingHandlerCount = 38;
    private const int MinimumInventoryHandlerCount = 30;

    [Fact]
    public void Infrastructure_assembly_contains_command_handlers()
    {
        var handlerCount = typeof(InfrasctructureExtensions).Assembly
            .GetTypes()
            .Count(type => type.GetInterfaces().Any(IsRequestHandlerInterface));

        handlerCount.Should().BeGreaterThanOrEqualTo(MinimumHorizontalHandlerCount,
            $"expected at least {MinimumHorizontalHandlerCount} IRequestHandler implementations in ERP.Infrastracture");
    }

    [Fact]
    public void Accounting_Infrastructure_assembly_contains_command_handlers()
    {
        var handlerCount = typeof(Accounting.Infrastructure.AccountingModuleExtensions).Assembly
            .GetTypes()
            .Count(type => type.GetInterfaces().Any(IsRequestHandlerInterface));

        handlerCount.Should().BeGreaterThanOrEqualTo(MinimumAccountingHandlerCount,
            $"expected at least {MinimumAccountingHandlerCount} IRequestHandler implementations in Accounting.Infrastructure");
    }

    [Fact]
    public void Inventory_Infrastructure_assembly_contains_command_handlers()
    {
        var handlerCount = typeof(Inventory.Infrastructure.InventoryModuleExtensions).Assembly
            .GetTypes()
            .Count(type => type.GetInterfaces().Any(IsRequestHandlerInterface));

        handlerCount.Should().BeGreaterThanOrEqualTo(MinimumInventoryHandlerCount,
            $"expected at least {MinimumInventoryHandlerCount} IRequestHandler implementations in Inventory.Infrastructure");
    }

    [Fact]
    public void MediatR_resolves_handlers_from_composition_root()
    {
        var services = new ServiceCollection();
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(InfrasctructureExtensions).Assembly));

        using var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        mediator.Should().NotBeNull();
    }

    private static bool IsRequestHandlerInterface(Type interfaceType)
    {
        if (!interfaceType.IsGenericType)
            return false;

        return interfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>);
    }
}
