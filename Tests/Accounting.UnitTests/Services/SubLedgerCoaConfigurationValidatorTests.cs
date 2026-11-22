using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Infrastructure.Services.SubLedger;
using ERP.BuildingBlocks.Application.Constants;
using FluentAssertions;
using Moq;

namespace Accounting.UnitTests.Services;

public class SubLedgerCoaConfigurationValidatorTests
{
    [Fact]
    public async Task ValidateAsync_When_all_required_bindings_exist_succeeds()
    {
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        repository
            .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildCompleteConfiguration());

        var validator = new SubLedgerCoaConfigurationValidator(repository.Object);

        await validator.Invoking(v => v.ValidateAsync()).Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateAsync_When_required_binding_missing_throws()
    {
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        repository
            .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<SubLedgerBaseAccount>());

        var validator = new SubLedgerCoaConfigurationValidator(repository.Object);

        var act = () => validator.ValidateAsync();

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().StartWith(ErrorMessages.SubLedgerBaseAccountNotConfigured);
    }

    [Fact]
    public async Task ValidateAsync_When_binding_combination_is_illegal_throws()
    {
        var repository = new Mock<ISubLedgerBaseAccountRepository>();
        var bindings = BuildCompleteConfiguration();
        bindings.Add(new SubLedgerBaseAccount
        {
            Id = Guid.NewGuid(),
            ChartOfAccountId = Guid.NewGuid(),
            SubLedgerType = SubLeadgerType.FixedAsset,
            Role = SubLedgerAccountRole.DepreciationExpense,
            FixedAssetType = FixedAssetType.Lands
        });

        repository
            .Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bindings);

        var validator = new SubLedgerCoaConfigurationValidator(repository.Object);

        var act = () => validator.ValidateAsync();

        (await act.Should().ThrowAsync<InvalidOperationException>())
            .Which.Message.Should().Be(SubLedgerBaseAccount.InvalidBindingMessage);
    }

    private static List<SubLedgerBaseAccount> BuildCompleteConfiguration()
    {
        var bindings = new List<SubLedgerBaseAccount>();

        foreach (SubLeadgerType type in Enum.GetValues<SubLeadgerType>())
        {
            if (type is SubLeadgerType.Tax or SubLeadgerType.FixedAsset)
                continue;

            bindings.Add(CreateBinding(type, SubLedgerAccountRole.Default, null));
        }

        bindings.Add(CreateBinding(SubLeadgerType.Branch, SubLedgerAccountRole.StockUnderTransfer, null));

        foreach (FixedAssetType assetType in Enum.GetValues<FixedAssetType>())
        {
            bindings.Add(CreateBinding(SubLeadgerType.FixedAsset, SubLedgerAccountRole.Asset, assetType));

            if (assetType == FixedAssetType.Lands)
                continue;

            bindings.Add(CreateBinding(
                SubLeadgerType.FixedAsset,
                SubLedgerAccountRole.AccumulatedDepreciation,
                assetType));
            bindings.Add(CreateBinding(
                SubLeadgerType.FixedAsset,
                SubLedgerAccountRole.DepreciationExpense,
                assetType));
        }

        return bindings;
    }

    private static SubLedgerBaseAccount CreateBinding(
        SubLeadgerType subLedgerType,
        SubLedgerAccountRole role,
        FixedAssetType? fixedAssetType)
        => SubLedgerBaseAccount.Create(Guid.NewGuid(), subLedgerType, role, fixedAssetType);
}
