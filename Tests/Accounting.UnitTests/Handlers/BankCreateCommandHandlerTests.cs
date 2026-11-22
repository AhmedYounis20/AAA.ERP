using Accounting.Application.Commands.Banks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
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
        var subLedgerCoaParentResolver = new Mock<ISubLedgerCoaParentResolver>();
        var subLedgerCoaFactory = new Mock<ISubLedgerCoaFactory>();

        repository
            .Setup(r => r.GetByNames("Existing Bank", "Existing Bank"))
            .ReturnsAsync(new Bank { Id = Guid.NewGuid(), Name = "Existing Bank", NameSecondLanguage = "Existing Bank" });

        var handler = new BankCreateCommandHandler(
            repository.Object,
            subLedgerCoaParentResolver.Object,
            subLedgerCoaFactory.Object);
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
        subLedgerCoaParentResolver.Verify(
            r => r.ResolveAsync(It.IsAny<SubLeadgerType>(), It.IsAny<FixedAssetType?>(), It.IsAny<SubLedgerAccountRole>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidDomainCommand_PersistsBank()
    {
        var repository = new Mock<IBankRepository>();
        var subLedgerCoaParentResolver = new Mock<ISubLedgerCoaParentResolver>();
        var subLedgerCoaFactory = new Mock<ISubLedgerCoaFactory>();
        var parentAccount = new ChartOfAccount { Id = Guid.NewGuid(), AccountNature = AccountNature.Debit };

        repository
            .Setup(r => r.GetByNames("New Bank", "New Bank"))
            .ReturnsAsync((Bank?)null);
        subLedgerCoaParentResolver
            .Setup(r => r.ResolveAsync(SubLeadgerType.Bank, null, SubLedgerAccountRole.Default, It.IsAny<CancellationToken>()))
            .ReturnsAsync(parentAccount);
        subLedgerCoaFactory
            .Setup(f => f.CreateChildAccountAsync(
                "New Bank",
                "New Bank",
                parentAccount,
                SubLeadgerType.Bank,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ChartOfAccount { Id = Guid.NewGuid(), Code = "1101" });

        var handler = new BankCreateCommandHandler(
            repository.Object,
            subLedgerCoaParentResolver.Object,
            subLedgerCoaFactory.Object);
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
