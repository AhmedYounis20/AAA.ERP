using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLedger;
using ERP.BuildingBlocks.Application.Constants;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Services;

public class SubLedgerCoaParentResolverTests
{
    [Fact]
    public async Task ResolveAsync_Returns_chart_account_for_single_binding()
    {
        var parentId = Guid.NewGuid();
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        repository
            .Setup(r => r.FindAsync(
                SubLeadgerType.Bank,
                SubLedgerAccountRole.Default,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new SubLedgerBaseAccount
                {
                    ChartOfAccountId = parentId,
                    SubLedgerType = SubLeadgerType.Bank,
                    Role = SubLedgerAccountRole.Default,
                    ChartOfAccount = new ChartOfAccount { Id = parentId }
                }
            ]);

        var resolver = new SubLedgerCoaParentResolver(repository.Object);

        var result = await resolver.ResolveAsync(SubLeadgerType.Bank);

        result.Id.Should().Be(parentId);
    }

    [Fact]
    public async Task ResolveAsync_When_not_configured_throws()
    {
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        repository
            .Setup(r => r.FindAsync(
                It.IsAny<SubLeadgerType>(),
                It.IsAny<SubLedgerAccountRole>(),
                It.IsAny<FixedAssetType?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<SubLedgerBaseAccount>());

        var resolver = new SubLedgerCoaParentResolver(repository.Object);

        var act = () => resolver.ResolveAsync(SubLeadgerType.Supplier);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().Be(ErrorMessages.SubLedgerBaseAccountNotConfigured);
    }

    [Fact]
    public async Task ResolveAsync_When_ambiguous_throws()
    {
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        repository
            .Setup(r => r.FindAsync(
                SubLeadgerType.Bank,
                SubLedgerAccountRole.Default,
                null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new SubLedgerBaseAccount
                {
                    ChartOfAccount = new ChartOfAccount { Id = Guid.NewGuid() },
                    SubLedgerType = SubLeadgerType.Bank,
                    Role = SubLedgerAccountRole.Default
                },
                new SubLedgerBaseAccount
                {
                    ChartOfAccount = new ChartOfAccount { Id = Guid.NewGuid() },
                    SubLedgerType = SubLeadgerType.Bank,
                    Role = SubLedgerAccountRole.Default
                }
            ]);

        var resolver = new SubLedgerCoaParentResolver(repository.Object);

        var act = () => resolver.ResolveAsync(SubLeadgerType.Bank);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().Be(ErrorMessages.SubLedgerBaseAccountAmbiguous);
    }
}
