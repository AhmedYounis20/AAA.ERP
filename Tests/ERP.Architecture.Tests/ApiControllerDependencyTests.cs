using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Phase 0 task 0.1.5 — API controllers must not depend on Infrastructure implementations.
/// Legacy exception: <see cref="ERP.API.Controllers.Account.DbworkController"/> (seed tooling; remove in Phase 3).
/// </summary>
public class ApiControllerDependencyTests
{
    [Fact]
    public void API_controllers_should_not_reference_Infrastructure_except_legacy_seed_controller()
    {
        var result = Types.InAssembly(typeof(ERP.API.Controllers.Inventory.ProductsController).Assembly)
            .That()
            .ResideInNamespaceStartingWith("ERP.API.Controllers")
            .And()
            .DoNotHaveName("DbworkController")
            .Should()
            .NotHaveDependencyOn("ERP.Persistence")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"API controller rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
