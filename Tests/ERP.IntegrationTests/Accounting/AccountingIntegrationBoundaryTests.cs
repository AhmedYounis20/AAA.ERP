using System.Net;
using Accounting.Contracts.Integration;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Entries;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Application.Constants;

namespace ERP.IntegrationTests.Accounting;

public class AccountingIntegrationBoundaryTests : IAsyncLifetime
{
  private IntegrationTestWebApplicationFactory _factory = null!;
  private AsyncServiceScope _scope;

  public async Task InitializeAsync()
  {
    _factory = new IntegrationTestWebApplicationFactory();
    _scope = _factory.Services.CreateAsyncScope();
    await InventoryTestDataSeeder.SeedAsync(_scope.ServiceProvider);
  }

  public async Task DisposeAsync()
  {
    await _scope.DisposeAsync();
    await _factory.DisposeAsync();
  }

  [Fact]
  public async Task GetOpenPeriodForDate_returns_seeded_period()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.GetOpenPeriodForDateAsync(InventoryTestIds.TransactionDate);

    result.IsSuccess.Should().BeTrue();
    result.Result!.Id.Should().Be(InventoryTestIds.FinancialPeriodId);
    result.Result.YearNumber.Should().Be("2026");
  }

  [Fact]
  public async Task GetOpenPeriodForDate_fails_when_no_matching_period()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.GetOpenPeriodForDateAsync(new DateTime(2099, 1, 1));

    result.IsSuccess.Should().BeFalse();
    result.Errors.Should().Contain(e => e.MessageKey == ErrorMessages.FinancialPeriodNotFound);
  }

  [Fact]
  public async Task GetBranchAccount_returns_chart_of_account_for_seeded_branch()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.GetBranchAccountAsync(InventoryTestIds.Branch1Id);

    result.IsSuccess.Should().BeTrue();
    result.Result!.ChartOfAccountId.Should().Be(InventoryTestIds.Branch1CoaId);
  }

  [Fact]
  public async Task GetBranchAccount_fails_for_unknown_branch()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.GetBranchAccountAsync(Guid.Parse("99999999-9999-9999-9999-999999999999"));

    result.IsSuccess.Should().BeFalse();
    result.Errors.Should().Contain(e => e.MessageKey == ErrorMessages.BranchOrBranchAccountNotFound);
  }

  [Fact]
  public async Task ValidatePartyAccount_succeeds_for_seeded_account()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.ValidatePartyAccountAsync(InventoryTestIds.PartyAccountId);

    result.IsSuccess.Should().BeTrue();
    result.Result.Should().Be(InventoryTestIds.PartyAccountId);
  }

  [Fact]
  public async Task ValidatePartyAccount_fails_for_unknown_account()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();

    var result = await service.ValidatePartyAccountAsync(Guid.Parse("99999999-9999-9999-9999-999999999999"));

    result.IsSuccess.Should().BeFalse();
    result.Errors.Should().Contain(e => e.MessageKey == ErrorMessages.PartyAccountNotFound);
  }

  [Fact]
  public async Task PostInventoryTransferEntry_creates_gl_entry_with_expected_accounts()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();
    var sourceDocumentId = Guid.NewGuid();
    const decimal amount = 75m;

    var result = await service.PostInventoryTransferEntryAsync(new InventoryTransferPostingRequest(
      sourceDocumentId,
      EntrySourceDocumentTypes.InventoryTransferStage,
      InventoryTestIds.Branch1Id,
      InventoryTestIds.Branch2CoaId,
      InventoryTestIds.StockUnderTransferCoaId,
      InventoryTestIds.TransactionDate,
      InventoryTestIds.FinancialPeriodId,
      amount));

    result.IsSuccess.Should().BeTrue();

    var entry = await InventoryTestDataSeeder.GetLatestEntryWithLinesAsync(_scope.ServiceProvider);
    entry.Should().NotBeNull();
    entry!.SourceDocumentId.Should().Be(sourceDocumentId);
    entry.SourceDocumentType.Should().Be(EntrySourceDocumentTypes.InventoryTransferStage);
    entry.FinancialTransactions.Should().HaveCount(2);

    var debit = entry.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Debit);
    var credit = entry.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Credit);
    debit.ChartOfAccountId.Should().Be(InventoryTestIds.Branch2CoaId);
    credit.ChartOfAccountId.Should().Be(InventoryTestIds.StockUnderTransferCoaId);
    debit.Amount.Should().Be(amount);
  }

  [Fact]
  public async Task PostInventoryTransferEntry_is_idempotent_by_source_document()
  {
    var service = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();
    var sourceDocumentId = Guid.NewGuid();

    var request = new InventoryTransferPostingRequest(
      sourceDocumentId,
      EntrySourceDocumentTypes.InventoryTransferComplete,
      InventoryTestIds.Branch1Id,
      InventoryTestIds.Branch1CoaId,
      InventoryTestIds.Branch2CoaId,
      InventoryTestIds.TransactionDate,
      InventoryTestIds.FinancialPeriodId,
      60m);

    var first = await service.PostInventoryTransferEntryAsync(request);
    var second = await service.PostInventoryTransferEntryAsync(request);

    first.IsSuccess.Should().BeTrue();
    second.IsSuccess.Should().BeTrue();
    second.Result.Should().Be(first.Result);

    var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
    entryCount.Should().Be(1);
  }
}
