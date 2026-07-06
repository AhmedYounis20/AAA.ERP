using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using FluentAssertions;
using Moq;
using Shared;

namespace ERP.UnitTests.CQRS.Handlers;

public class BaseSettingCreateHandlerTests
{
    public sealed class TestSettingEntity : BaseSettingEntity;

    private sealed class TestCreateCommand : BaseSettingCreateCommand<TestSettingEntity>
    {
        public TestCreateCommand(string name, string nameSecondLanguage)
        {
            Name = name;
            NameSecondLanguage = nameSecondLanguage;
        }
    }

    private sealed class TestCreateHandler(IBaseSettingRepository<TestSettingEntity> repository)
        : BaseSettingCreateHandler<TestSettingEntity, TestCreateCommand>(repository);

    [Fact]
    public async Task Handle_creates_entity_when_validation_passes()
    {
        var repository = new Mock<IBaseSettingRepository<TestSettingEntity>>();
        repository.Setup(r => r.GetByNames(It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((TestSettingEntity?)null);
        repository.Setup(r => r.Add(It.IsAny<TestSettingEntity>()))
            .ReturnsAsync((TestSettingEntity e) => e);

        var handler = new TestCreateHandler(repository.Object);
        var command = new TestCreateCommand("Role A", "دور أ");

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Result!.Name.Should().Be("Role A");
        repository.Verify(r => r.Add(It.IsAny<TestSettingEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_returns_bad_request_when_duplicate_name_exists()
    {
        var existing = new TestSettingEntity { Name = "Role A", NameSecondLanguage = "x" };
        var repository = new Mock<IBaseSettingRepository<TestSettingEntity>>();
        repository.Setup(r => r.GetByNames(It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(existing);

        var handler = new TestCreateHandler(repository.Object);
        var command = new TestCreateCommand("Role A", "دور ب");

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "WithSameNameIsExisted");
        repository.Verify(r => r.Add(It.IsAny<TestSettingEntity>()), Times.Never);
    }
}

public class BaseSettingUpdateHandlerTests
{
    public sealed class TestSettingEntity : BaseSettingEntity;

    private sealed class TestUpdateCommand : BaseSettingUpdateCommand<TestSettingEntity>
    {
        public TestUpdateCommand(Guid id, string name, string nameSecondLanguage)
        {
            Id = id;
            Name = name;
            NameSecondLanguage = nameSecondLanguage;
        }
    }

    private sealed class TestUpdateHandler(IBaseSettingRepository<TestSettingEntity> repository)
        : BaseSettingUpdateHandler<TestSettingEntity, TestUpdateCommand>(repository);

    [Fact]
    public async Task Handle_updates_entity_when_validation_passes()
    {
        var entityId = Guid.NewGuid();
        var existing = new TestSettingEntity
        {
            Id = entityId,
            Name = "Old",
            NameSecondLanguage = "قديم"
        };

        var repository = new Mock<IBaseSettingRepository<TestSettingEntity>>();
        repository.Setup(r => r.Get(entityId)).ReturnsAsync(existing);
        repository.Setup(r => r.GetByNames(It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((TestSettingEntity?)null);
        repository.Setup(r => r.Update(It.IsAny<TestSettingEntity>()))
            .ReturnsAsync((TestSettingEntity e) => e);

        var handler = new TestUpdateHandler(repository.Object);
        var command = new TestUpdateCommand(entityId, "New", "جديد");

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Result!.Name.Should().Be("New");
        repository.Verify(r => r.Update(It.IsAny<TestSettingEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_returns_bad_request_when_duplicate_name_exists_on_another_entity()
    {
        var entityId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var existing = new TestSettingEntity { Id = entityId, Name = "Mine", NameSecondLanguage = "خاص" };
        var duplicate = new TestSettingEntity { Id = otherId, Name = "Taken", NameSecondLanguage = "مأخوذ" };

        var repository = new Mock<IBaseSettingRepository<TestSettingEntity>>();
        repository.Setup(r => r.Get(entityId)).ReturnsAsync(existing);
        repository.Setup(r => r.GetByNames("Taken", "مأخوذ")).ReturnsAsync(duplicate);

        var handler = new TestUpdateHandler(repository.Object);
        var command = new TestUpdateCommand(entityId, "Taken", "مأخوذ");

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "WithSameNameIsExisted");
        repository.Verify(r => r.Update(It.IsAny<TestSettingEntity>()), Times.Never);
    }
}

public class BaseTreeSettingDeleteHandlerTests
{
    public sealed class TestTreeSettingEntity : BaseTreeSettingEntity<TestTreeSettingEntity>;

    private sealed record TestDeleteCommand(Guid Id) : ICommand<ApiResponse<TestTreeSettingEntity>>, IEntityDeleteCommand;

    private sealed class TestDeleteHandler(IBaseTreeSettingRepository<TestTreeSettingEntity> repository)
        : BaseTreeSettingDeleteHandler<TestTreeSettingEntity, TestDeleteCommand>(repository);

    [Fact]
    public async Task Handle_returns_bad_request_when_entity_has_children()
    {
        var entityId = Guid.NewGuid();
        var repository = new Mock<IBaseTreeSettingRepository<TestTreeSettingEntity>>();
        repository.Setup(r => r.HasChildren(entityId)).ReturnsAsync(true);

        var handler = new TestDeleteHandler(repository.Object);
        var response = await handler.Handle(new TestDeleteCommand(entityId), CancellationToken.None);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "CannotDeleteParent");
        repository.Verify(r => r.Delete(It.IsAny<TestTreeSettingEntity>()), Times.Never);
    }

    [Fact]
    public async Task Handle_deletes_entity_when_no_children()
    {
        var entityId = Guid.NewGuid();
        var entity = new TestTreeSettingEntity { Id = entityId, Name = "Node", NameSecondLanguage = "عقدة" };

        var repository = new Mock<IBaseTreeSettingRepository<TestTreeSettingEntity>>();
        repository.Setup(r => r.HasChildren(entityId)).ReturnsAsync(false);
        repository.Setup(r => r.Get(entityId)).ReturnsAsync(entity);
        repository.Setup(r => r.Delete(entity)).Returns(Task.CompletedTask);

        var handler = new TestDeleteHandler(repository.Object);
        var response = await handler.Handle(new TestDeleteCommand(entityId), CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        repository.Verify(r => r.Delete(entity), Times.Once);
    }
}
