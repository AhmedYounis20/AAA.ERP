using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLedger;
using ERP.BuildingBlocks.Application.Constants;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Services;

public class SubLedgerCoaFactoryTests
{
    [Fact]
    public async Task CreateChildAccountAsync_Inherits_parent_properties()
    {
        var parentId = Guid.NewGuid();
        var parent = new ChartOfAccount
        {
            Id = parentId,
            AccountNature = AccountNature.Credit,
            IsDepreciable = true,
            IsActiveAccount = true,
            IsStopDealing = false,
            IsPostedAccount = true,
            AccountGuidId = Guid.NewGuid()
        };

        var repository = new Mock<IChartOfAccountRepository>();
        repository
            .Setup(r => r.GenerateNewCodeForChild(parentId))
            .ReturnsAsync("1010020001");

        var factory = new SubLedgerCoaFactory(repository.Object);

        var child = await factory.CreateChildAccountAsync(
            "ACME",
            "ACME",
            parent,
            SubLeadgerType.Customer);

        child.ParentId.Should().Be(parentId);
        child.Code.Should().Be("1010020001");
        child.AccountNature.Should().Be(AccountNature.Credit);
        child.IsDepreciable.Should().BeTrue();
        child.IsCreatedFromSubLeadger.Should().BeTrue();
        child.SubLeadgerType.Should().Be(SubLeadgerType.Customer);
    }

    [Fact]
    public async Task CreatePrefixedChildAccountAsync_copies_parent_subledger_type()
    {
        var parentId = Guid.NewGuid();
        var parent = new ChartOfAccount
        {
            Id = parentId,
            Name = "Accumulated",
            NameSecondLanguage = "Accumulated",
            AccountNature = AccountNature.Credit,
            SubLeadgerType = SubLeadgerType.FixedAsset,
            AccountGuidId = Guid.NewGuid()
        };

        var repository = new Mock<IChartOfAccountRepository>();
        repository
            .Setup(r => r.GenerateNewCodeForChild(parentId))
            .ReturnsAsync("1020010002001");

        var factory = new SubLedgerCoaFactory(repository.Object);

        var child = await factory.CreatePrefixedChildAccountAsync(parent, "Printer", "Printer", parent);

        child.IsCreatedFromSubLeadger.Should().BeTrue();
        child.SubLeadgerType.Should().Be(SubLeadgerType.FixedAsset);
        child.Name.Should().Be("Accumulated Printer");
    }

    [Fact]
    public async Task CreateChildAccountAsync_When_parent_null_throws()
    {
        var factory = new SubLedgerCoaFactory(Mock.Of<IChartOfAccountRepository>());

        var act = () => factory.CreateChildAccountAsync("Name", "Name", null!, SubLeadgerType.Bank);

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().Be(ErrorMessages.SubLedgerParentAccountNotFound);
    }
}
