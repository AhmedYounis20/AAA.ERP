using ERP.Application.Data;
using ERP.Application.Services.Caching;
using ERP.Domain.Models.Entities.Account.Currencies;
using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Enforces clean-architecture layer dependency rules (Phase 0 — MODULAR_MONOLITH.md §10.2).
/// </summary>
public class LayerDependencyTests
{
    private const string InfrastructureAssembly = "ERP.Infrastracture";
    private const string ApiAssembly = "ERP.API";

    [Fact]
    public void Domain_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Currency).Assembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Application_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(IApplicationDbContext).Assembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Domain_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(Currency).Assembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Application_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(IApplicationDbContext).Assembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Infrastructure_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(ERP.Infrastracture.Utilities.InfrasctructureExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Shared_should_not_reference_Application_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::Shared.BaseEntities.Identity.ApplicationUser).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Application")
            .And()
            .NotHaveDependencyOn("ERP.Domain")
            .And()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void BuildingBlocks_Domain_should_not_reference_Application_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::Shared.BaseEntities.BaseEntity).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Application")
            .And()
            .NotHaveDependencyOn("ERP.Domain")
            .And()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void BuildingBlocks_Application_should_not_reference_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::Shared.DTOs.PaginatedResult<>).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Domain")
            .And()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Architecture rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
