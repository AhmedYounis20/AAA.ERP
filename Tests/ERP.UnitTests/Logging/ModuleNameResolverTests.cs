using ERP.BuildingBlocks.Application.Logging;
using FluentAssertions;
using Inventory.Application.Commands.Colors;

namespace ERP.UnitTests.Logging;

public class ModuleNameResolverTests
{
    [Theory]
    [InlineData(typeof(ColorCreateCommand), "Inventory")]
    [InlineData(typeof(Accounting.Application.Commands.Taxes.TaxCreateCommand), "Accounting")]
    public void FromType_resolves_module_from_command_namespace(Type type, string expectedModule)
    {
        ModuleNameResolver.FromType(type).Should().Be(expectedModule);
    }

    [Fact]
    public void FromType_resolves_legacy_accounting_commands()
    {
        ModuleNameResolver.FromType(typeof(global::Domain.Account.Commands.BaseInputModels.BaseCreateCommands.BaseCreateCommand<>)
                .GetGenericTypeDefinition())
            .Should().Be("Accounting");
    }

    [Theory]
    [InlineData(typeof(ERP.API.Controllers.Inventory.ColorsController), "Inventory")]
    [InlineData(typeof(ERP.API.Controllers.Account.CurrenciesController), "Accounting")]
    [InlineData(typeof(ERP.API.Controllers.Identity.AuthController), "Identity")]
    [InlineData(typeof(ERP.API.Controllers.Audit.AuditLogsController), "Platform")]
    public void FromType_resolves_controller_namespace(Type type, string expectedModule)
    {
        ModuleNameResolver.FromType(type).Should().Be(expectedModule);
    }
}
