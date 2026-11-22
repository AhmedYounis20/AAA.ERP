using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Infrastructure.Services.SubLedger;
using ERP.BuildingBlocks.Application.Constants;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Services;

public class SubLedgerCoaParentValidatorTests
{
    [Fact]
    public async Task ValidateAndGetAsync_When_valid_parent_returns_account()
    {
        var parentId = Guid.NewGuid();
        var account = new ChartOfAccount
        {
            Id = parentId,
            IsActiveAccount = true,
            IsStopDealing = false,
            IsCreatedFromSubLeadger = false
        };

        var repository = new Mock<IChartOfAccountRepository>();
        repository.Setup(r => r.Get(parentId)).ReturnsAsync(account);

        var validator = new SubLedgerCoaParentValidator(repository.Object);

        var result = await validator.ValidateAndGetAsync(parentId);

        result.Id.Should().Be(parentId);
    }

    [Fact]
    public async Task ValidateAndGetAsync_When_created_from_subledger_throws()
    {
        var parentId = Guid.NewGuid();
        var repository = new Mock<IChartOfAccountRepository>();
        repository.Setup(r => r.Get(parentId)).ReturnsAsync(new ChartOfAccount
        {
            Id = parentId,
            IsActiveAccount = true,
            IsStopDealing = false,
            IsCreatedFromSubLeadger = true
        });

        var validator = new SubLedgerCoaParentValidator(repository.Object);

        var act = () => validator.ValidateAndGetAsync(parentId);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().Be(ErrorMessages.SubLedgerParentAccountInvalid);
    }
}
