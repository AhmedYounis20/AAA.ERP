using ERP.BuildingBlocks.Persistence.Data;
using Accounting.Domain.Models.Entities.Entries;
using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Enforces clean-architecture layer dependency rules (Phase 0 — MODULAR_MONOLITH.md §10.2).
/// </summary>
public class LayerDependencyTests
{
    private const string InfrastructureAssembly = "ERP.Persistence";
    private const string ApiAssembly = "ERP.Host";

    [Fact]
    public void Domain_should_not_reference_Infrastructure()
    {
        var result = Types.InAssembly(typeof(Entry).Assembly)
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
        var result = Types.InAssembly(typeof(Entry).Assembly)
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
        var result = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Shared_should_not_reference_Application_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::ERP.BuildingBlocks.Application.Constants.ErrorMessages).Assembly)
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
    public void Identity_Domain_should_not_reference_Application_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::ERP.BuildingBlocks.Domain.Entities.Identity.ApplicationUser).Assembly)
            .Should()
            .NotHaveDependencyOn("ERP.Application")
            .And()
            .NotHaveDependencyOn("ERP.Domain")
            .And()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .And()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Identity_Application_should_not_reference_Infrastructure_or_API()
    {
        var result = Types.InAssembly(typeof(global::Identity.Application.IAuthenticationService).Assembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly)
            .And()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void Identity_Infrastructure_should_not_reference_API()
    {
        var result = Types.InAssembly(typeof(global::Identity.Infrastructure.IdentityModuleExtensions).Assembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void BuildingBlocks_Domain_should_not_reference_Application_Domain_or_Infrastructure()
    {
        var result = Types.InAssembly(typeof(global::ERP.BuildingBlocks.Domain.Entities.BaseEntity).Assembly)
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
        var result = Types.InAssembly(typeof(global::ERP.BuildingBlocks.Application.DTOs.PaginatedResult<>).Assembly)
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
