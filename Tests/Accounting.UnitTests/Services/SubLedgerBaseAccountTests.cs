using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using FluentAssertions;

namespace Accounting.UnitTests.Services;

public class SubLedgerBaseAccountTests
{
    [Fact]
    public void Create_accepts_bank_default_binding()
    {
        var accountId = Guid.NewGuid();

        var binding = SubLedgerBaseAccount.Create(
            accountId,
            SubLeadgerType.Bank,
            SubLedgerAccountRole.Default,
            null);

        binding.Id.Should().Be(accountId);
        binding.ChartOfAccountId.Should().Be(accountId);
        binding.FixedAssetType.Should().BeNull();
    }

    [Theory]
    [InlineData(SubLeadgerType.Customer, SubLedgerAccountRole.StockUnderTransfer)]
    [InlineData(SubLeadgerType.Bank, SubLedgerAccountRole.Asset)]
    [InlineData(SubLeadgerType.Tax, SubLedgerAccountRole.Default)]
    public void Create_rejects_role_that_does_not_belong_to_the_subledger(
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role)
    {
        var act = () => SubLedgerBaseAccount.Create(Guid.NewGuid(), subLedgerType, role, null);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(SubLedgerBaseAccount.InvalidBindingMessage);
    }

    [Fact]
    public void Create_rejects_fixed_asset_type_on_a_non_asset_binding()
    {
        var act = () => SubLedgerBaseAccount.Create(
            Guid.NewGuid(),
            SubLeadgerType.Customer,
            SubLedgerAccountRole.Default,
            FixedAssetType.Buildings);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(SubLedgerBaseAccount.InvalidBindingMessage);
    }

    [Fact]
    public void Create_rejects_depreciation_role_for_lands()
    {
        var act = () => SubLedgerBaseAccount.Create(
            Guid.NewGuid(),
            SubLeadgerType.FixedAsset,
            SubLedgerAccountRole.AccumulatedDepreciation,
            FixedAssetType.Lands);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage(SubLedgerBaseAccount.InvalidBindingMessage);
    }

    [Fact]
    public void Create_accepts_buildings_depreciation_expense()
    {
        var binding = SubLedgerBaseAccount.Create(
            Guid.NewGuid(),
            SubLeadgerType.FixedAsset,
            SubLedgerAccountRole.DepreciationExpense,
            FixedAssetType.Buildings);

        binding.FixedAssetType.Should().Be(FixedAssetType.Buildings);
        binding.Role.Should().Be(SubLedgerAccountRole.DepreciationExpense);
    }
}
