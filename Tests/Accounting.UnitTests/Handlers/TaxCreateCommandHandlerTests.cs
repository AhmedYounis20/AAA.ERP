using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Infrastructure.Handlers.Taxes;
using ERP.BuildingBlocks.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Handlers;

public class TaxCreateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidDomainCommand_PersistsTax()
    {
        var repository = new Mock<ITaxRepository>();
        var chartOfAccountRepository = new Mock<IChartOfAccountRepository>();
        var parentId = Guid.NewGuid();

        chartOfAccountRepository.Setup(r => r.Get(parentId)).ReturnsAsync(new ChartOfAccount { Id = parentId });
        chartOfAccountRepository.Setup(r => r.GenerateNewCodeForChild(parentId)).ReturnsAsync("9001");

        var handler = new TaxCreateCommandHandler(repository.Object, chartOfAccountRepository.Object);
        var command = new TaxCreateCommand
        {
            Name = "VAT",
            NameSecondLanguage = "VAT",
            NodeType = NodeType.Domain,
            ChartOfAccountId = parentId,
            Percent = 14m
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Result.Should().NotBeNull();
        repository.Verify(r => r.Add(It.IsAny<Tax>()), Times.Once);
    }
}
