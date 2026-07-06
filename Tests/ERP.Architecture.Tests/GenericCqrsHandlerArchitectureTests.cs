using Accounting.Infrastructure.Handlers.Roles;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Base;
using FluentAssertions;
using Inventory.Infrastructure.Handlers.PackingUnits;
using System.Reflection;

namespace ERP.Architecture.Tests;

/// <summary>
/// ADR-013: generic CRUD handlers must depend on repositories, not legacy write services.
/// </summary>
public class GenericCqrsHandlerArchitectureTests
{
    private static readonly Type CrudHandlerExecutionBaseDefinition =
        typeof(CrudHandlerExecutionBase<>);

    private static readonly Lazy<Type[]> GenericCrudHandlers = new(DiscoverGenericCrudHandlers);

    private static Type[] DiscoverGenericCrudHandlers()
    {
        return new[]
            {
                typeof(RoleCreateCommandHandler).Assembly,
                typeof(PackingUnitCreateCommandHandler).Assembly
            }
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && InheritsCrudHandlerExecutionBase(type))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToArray();
    }

    [Fact]
    public void Generic_crud_handlers_should_be_discovered()
    {
        GenericCrudHandlers.Value.Should().HaveCountGreaterThanOrEqualTo(24,
            "expected migrated Create/Update/Delete handlers across Phase 1.1 and 1.2");
    }

    [Fact]
    public void Generic_crud_handlers_should_not_inject_legacy_write_services()
    {
        foreach (var handler in GenericCrudHandlers.Value)
        {
            var writeServiceParams = handler.GetConstructors()
                .SelectMany(c => c.GetParameters())
                .Where(IsLegacyWriteServiceParameter)
                .Select(p => p.ParameterType.Name)
                .Distinct()
                .ToList();

            writeServiceParams.Should().BeEmpty(
                $"{handler.Name} inherits generic CRUD handler base and must inject repositories only. " +
                $"Found: {string.Join(", ", writeServiceParams)}");
        }
    }

    private static bool InheritsCrudHandlerExecutionBase(Type type)
    {
        for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (baseType.IsGenericType
                && baseType.GetGenericTypeDefinition() == CrudHandlerExecutionBaseDefinition)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsLegacyWriteServiceParameter(ParameterInfo parameter)
    {
        var typeName = parameter.ParameterType.Name;

        if (!typeName.EndsWith("Service", StringComparison.Ordinal))
            return false;

        if (typeName.Contains("Queries", StringComparison.Ordinal)
            || typeName.Contains("QueryService", StringComparison.Ordinal)
            || typeName is "IAuthenticationService")
        {
            return false;
        }

        return parameter.ParameterType.Namespace?.Contains(".Application.", StringComparison.Ordinal) == true;
    }
}
