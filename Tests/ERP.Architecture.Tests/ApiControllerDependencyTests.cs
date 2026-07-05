using Accounting.Api;
using FluentAssertions;
using Identity.Api;
using Inventory.Api;
using NetArchTest.Rules;
using Platform.Audit.Api;

namespace ERP.Architecture.Tests;

/// <summary>
/// API controllers must not depend on Infrastructure implementations.
/// Legacy exception: <see cref="ERP.Host.Dev.DbWorkController"/> in Host (development seed tooling).
/// </summary>
public class ApiControllerDependencyTests
{
    [Fact]
    public void Module_API_controllers_should_not_reference_Persistence()
    {
        foreach (var assembly in new[]
        {
            IdentityApiAssemblyMarker.Assembly,
            AccountingApiAssemblyMarker.Assembly,
            InventoryApiAssemblyMarker.Assembly,
            PlatformAuditApiAssemblyMarker.Assembly
        })
        {
            var result = Types.InAssembly(assembly)
                .That()
                .ResideInNamespaceEndingWith("Controllers")
                .Should()
                .NotHaveDependencyOn("ERP.Persistence")
                .GetResult();

            result.IsSuccessful.Should().BeTrue(FormatViolationMessage(assembly.GetName().Name!, result));
        }
    }

    [Fact]
    public void Host_except_Dev_should_not_reference_Persistence()
    {
        var result = Types.InAssembly(typeof(ERP.Host.Dev.DbWorkController).Assembly)
            .That()
            .DoNotResideInNamespace("ERP.Host.Dev")
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage("ERP.Host", result));
    }

    private static string FormatViolationMessage(string scope, TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"API controller rule violated ({scope}). Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
