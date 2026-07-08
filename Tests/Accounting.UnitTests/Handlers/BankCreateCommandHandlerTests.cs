using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Handlers.Banks;
using ERP.BuildingBlocks.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Handlers;

public class BankCreateCommandHandlerTests
{
    [Fact]
    public async Task Handle_DuplicateName_ReturnsValidationError()
    {
        var repository = new Mock<IBankRepository>();
        var chartOfAccountRepository = new Mock<IChartOfAccountRepository>();
        var parentId = Guid.Parse("0aecd366-95fd-47b8-8516-08dc3dfcb068");

        repository
            .Setup(r => r.GetByNames("Existing Bank", "Existing Bank"))
            .ReturnsAsync(new Bank { Id = Guid.NewGuid(), Name = "Existing Bank", NameSecondLanguage = "Existing Bank" });

        var handler = new BankCreateCommandHandler(repository.Object, chartOfAccountRepository.Object);
        var command = new BankCreateCommand
        {
            Name = "Existing Bank",
            NameSecondLanguage = "Existing Bank",
            NodeType = NodeType.Domain
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.MessageKey == "WithSameNameIsExisted");
        repository.Verify(r => r.Add(It.IsAny<Bank>()), Times.Never);
        chartOfAccountRepository.Verify(r => r.Get(It.IsAny<Guid?>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidDomainCommand_PersistsBank()
    {
        var repository = new Mock<IBankRepository>();
        var chartOfAccountRepository = new Mock<IChartOfAccountRepository>();

        repository
            .Setup(r => r.GetByNames("New Bank", "New Bank"))
            .ReturnsAsync((Bank?)null);
        chartOfAccountRepository
            .Setup(r => r.Get(It.IsAny<Guid?>()))
            .ReturnsAsync(new ChartOfAccount { Id = Guid.NewGuid(), AccountNature = AccountNature.Debit });
        chartOfAccountRepository
            .Setup(r => r.GenerateNewCodeForChild(It.IsAny<Guid?>()))
            .ReturnsAsync("1101");

        var handler = new BankCreateCommandHandler(repository.Object, chartOfAccountRepository.Object);
        var command = new BankCreateCommand
        {
            Name = "New Bank",
            NameSecondLanguage = "New Bank",
            NodeType = NodeType.Domain
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        repository.Verify(r => r.Add(It.IsAny<Bank>()), Times.Once);
    }
}
