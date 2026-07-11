using ERP.BuildingBlocks.Application.Logging;
using FluentAssertions;
using Inventory.Application.Commands.PackingUnits;

namespace ERP.UnitTests.Logging;

public class ModuleNameResolverTests
{
    [Theory]
    [InlineData(typeof(PackingUnitCreateCommand), "Inventory")]
    [InlineData(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand), "Accounting")]
    public void FromType_resolves_module_from_command_namespace(Type type, string expectedModule)
    {
        ModuleNameResolver.FromType(type).Should().Be(expectedModule);
    }

    [Fact]
    public void FromType_resolves_building_blocks_base_commands_as_ERP()
    {
        ModuleNameResolver.FromType(typeof(global::ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands.BaseCreateCommand<>)
                .GetGenericTypeDefinition())
            .Should().Be("ERP");
    }

    [Theory]
    [InlineData(typeof(Inventory.Api.Controllers.Catalog.ProductsController), "Inventory")]
    [InlineData(typeof(Accounting.Api.Controllers.Settings.CurrenciesController), "Accounting")]
    [InlineData(typeof(Identity.Api.Controllers.AuthController), "Identity")]
    [InlineData(typeof(Platform.Audit.Api.Controllers.AuditLogsController), "Platform")]
    public void FromType_resolves_controller_namespace(Type type, string expectedModule)
    {
        ModuleNameResolver.FromType(type).Should().Be(expectedModule);
    }
}
