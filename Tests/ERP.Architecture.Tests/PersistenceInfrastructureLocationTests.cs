using FluentAssertions;
using NetArchTest.Rules;

namespace ERP.Architecture.Tests;

/// <summary>
/// Ensures generic persistence infrastructure lives in BuildingBlocks.Persistence, not ERP.Persistence.
/// UnitOfWork and TransactionCoordinator remain in ERP.Persistence until IApplicationDbContext is redesigned.
/// </summary>
public class PersistenceInfrastructureLocationTests
{
    [Fact]
    public void BaseRepository_should_reside_in_BuildingBlocks_Persistence()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Persistence.Repositories.BaseRepository<>).Assembly)
            .That()
            .HaveName("BaseRepository")
            .Should()
            .ResideInNamespace("ERP.BuildingBlocks.Persistence.Repositories")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    [Fact]
    public void ERP_Persistence_should_not_contain_BaseRepository_implementation()
    {
        var types = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .That()
            .HaveName("BaseRepository")
            .GetTypes();

        types.Should().BeEmpty("BaseRepository belongs in ERP.BuildingBlocks.Persistence");
    }

    [Fact]
    public void ERP_Persistence_should_not_contain_BaseQueryService_implementation()
    {
        var types = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .That()
            .HaveName("BaseQueryService")
            .GetTypes();

        types.Should().BeEmpty("BaseQueryService belongs in ERP.BuildingBlocks.Persistence");
    }

    [Fact]
    public void ERP_Persistence_should_still_contain_UnitOfWork_and_TransactionCoordinator()
    {
        var unitOfWork = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .That()
            .HaveName("UnitOfWork")
            .GetTypes();

        var coordinator = Types.InAssembly(typeof(ERP.Infrastructure.Utilities.InfrastructureExtensions).Assembly)
            .That()
            .HaveName("TransactionCoordinator")
            .GetTypes();

        unitOfWork.Should().ContainSingle();
        coordinator.Should().ContainSingle();
    }

    [Fact]
    public void Base_EF_configurations_should_reside_in_BuildingBlocks_Persistence()
    {
        var result = Types.InAssembly(typeof(ERP.BuildingBlocks.Persistence.Configurations.BaseEntityDbConfig<>).Assembly)
            .That()
            .HaveNameEndingWith("DbConfig")
            .And()
            .HaveNameStartingWith("Base")
            .Should()
            .ResideInNamespace("ERP.BuildingBlocks.Persistence.Configurations")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(FormatViolationMessage(result));
    }

    private static string FormatViolationMessage(TestResult result)
    {
        if (result.IsSuccessful || result.FailingTypes is null || !result.FailingTypes.Any())
            return string.Empty;

        var typeNames = result.FailingTypes.Select(t => t.FullName ?? t.Name);
        return $"Persistence infrastructure location rule violated. Failing types:{Environment.NewLine}{string.Join(Environment.NewLine, typeNames)}";
    }
}
