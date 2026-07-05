using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using Accounting.Contracts.Integration;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Entries;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.IntegrationTests.Inventory;

public class InventoryTransactionIntegrationTests : IAsyncLifetime
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
    public async Task Receipt_increases_stock_and_creates_gl_entry()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IInventoryTransactionService>();
        const decimal quantity = 5;
        const decimal totalCost = 50;

        var result = await service.Create(new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = InventoryTestIds.TransactionDate,
            BranchId = InventoryTestIds.Branch1Id,
            TransactionPartyId = InventoryTestIds.PartyAccountId,
            Items =
            [
                new InventoryTransactionItemCreateDto
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = quantity,
                    TotalCost = totalCost
                }
            ]
        });

        result.IsSuccess.Should().BeTrue(result.Errors?.FirstOrDefault()?.MessageKey);

        var stock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        stock.Should().Be(quantity);

        var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entryCount.Should().Be(1);

        var entry = await InventoryTestDataSeeder.GetLatestEntryWithLinesAsync(_scope.ServiceProvider);
        entry.Should().NotBeNull();
        entry!.FinancialTransactions.Should().HaveCount(2);

        AssertReceiptGlLines(entry, totalCost);
    }

    [Fact]
    public async Task Receipt_gl_failure_rolls_back_stock_and_entry()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IInventoryTransactionService>();
        var invalidPartyId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var result = await service.Create(new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = InventoryTestIds.TransactionDate,
            BranchId = InventoryTestIds.Branch1Id,
            TransactionPartyId = invalidPartyId,
            Items =
            [
                new InventoryTransactionItemCreateDto
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = 3,
                    TotalCost = 30
                }
            ]
        });

        result.IsSuccess.Should().BeFalse();

        var stock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            _scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        stock.Should().Be(0);

        var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entryCount.Should().Be(0);

        var transactionCount = await InventoryTestDataSeeder.CountTransactionsAsync(_scope.ServiceProvider);
        transactionCount.Should().Be(0);
    }

    [Fact]
    public async Task Issue_with_insufficient_stock_fails_without_gl_entry()
    {
        await using var factory = new IntegrationTestWebApplicationFactory();
        await using var scope = factory.Services.CreateAsyncScope();
        await InventoryTestDataSeeder.SeedAsync(scope.ServiceProvider, initialStockBalance: 2);

        var service = scope.ServiceProvider.GetRequiredService<IInventoryTransactionService>();

        var result = await service.Create(new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Issue,
            TransactionDate = InventoryTestIds.TransactionDate,
            BranchId = InventoryTestIds.Branch1Id,
            TransactionPartyId = InventoryTestIds.PartyAccountId,
            Items =
            [
                new InventoryTransactionItemCreateDto
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = 10,
                    TotalCost = 100
                }
            ]
        });

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.MessageKey == "InsufficientStockBalance");

        var stock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        stock.Should().Be(2);

        var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(scope.ServiceProvider);
        entryCount.Should().Be(0);
    }

    [Fact]
    public async Task Receipt_gl_lines_match_expected_branch_and_party_accounts()
    {
        var service = _scope.ServiceProvider.GetRequiredService<IInventoryTransactionService>();
        const decimal totalCost = 120;

        var result = await service.Create(new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Receipt,
            TransactionDate = InventoryTestIds.TransactionDate,
            BranchId = InventoryTestIds.Branch1Id,
            TransactionPartyId = InventoryTestIds.PartyAccountId,
            Items =
            [
                new InventoryTransactionItemCreateDto
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = 12,
                    TotalCost = totalCost
                }
            ]
        });

        result.IsSuccess.Should().BeTrue();

        var entry = await InventoryTestDataSeeder.GetLatestEntryWithLinesAsync(_scope.ServiceProvider);
        AssertReceiptGlLines(entry!, totalCost);
    }

    [Fact]
    public async Task Duplicate_gl_post_for_same_source_document_is_idempotent()
    {
        var integration = _scope.ServiceProvider.GetRequiredService<IAccountingIntegrationService>();
        var sourceDocumentId = Guid.NewGuid();

        var request = new InventoryTransactionPostingRequest(
            sourceDocumentId,
            InventoryTestIds.Branch1Id,
            InventoryTestIds.PartyAccountId,
            InventoryTestIds.TransactionDate,
            InventoryTestIds.FinancialPeriodId,
            50m,
            InventoryGlDirection.Receipt);

        var first = await integration.PostInventoryTransactionEntryAsync(request);
        var second = await integration.PostInventoryTransactionEntryAsync(request);

        first.IsSuccess.Should().BeTrue();
        second.IsSuccess.Should().BeTrue();
        second.Result.Should().Be(first.Result);

        var entryCount = await InventoryTestDataSeeder.CountEntriesAsync(_scope.ServiceProvider);
        entryCount.Should().Be(1);
    }

    [Fact]
    public async Task Issue_with_sufficient_stock_decreases_balance_and_posts_gl()
    {
        await using var factory = new IntegrationTestWebApplicationFactory();
        await using var scope = factory.Services.CreateAsyncScope();
        await InventoryTestDataSeeder.SeedAsync(scope.ServiceProvider, initialStockBalance: 20);

        var service = scope.ServiceProvider.GetRequiredService<IInventoryTransactionService>();
        const decimal issueQuantity = 8;
        const decimal totalCost = 80;

        var result = await service.Create(new InventoryTransactionCreateCommand
        {
            TransactionType = InventoryTransactionType.Issue,
            TransactionDate = InventoryTestIds.TransactionDate,
            BranchId = InventoryTestIds.Branch1Id,
            TransactionPartyId = InventoryTestIds.PartyAccountId,
            Items =
            [
                new InventoryTransactionItemCreateDto
                {
                    VariantId = InventoryTestIds.VariantId,
                    PackingUnitId = InventoryTestIds.PackingUnitId,
                    Quantity = issueQuantity,
                    TotalCost = totalCost
                }
            ]
        });

        result.IsSuccess.Should().BeTrue(result.Errors?.FirstOrDefault()?.MessageKey);

        var stock = await InventoryTestDataSeeder.GetStockBalanceAsync(
            scope.ServiceProvider,
            InventoryTestIds.Branch1Id);
        stock.Should().Be(12);

        var entry = await InventoryTestDataSeeder.GetLatestEntryWithLinesAsync(scope.ServiceProvider);
        entry.Should().NotBeNull();

        var debit = entry!.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Debit);
        var credit = entry.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Credit);
        debit.ChartOfAccountId.Should().Be(InventoryTestIds.PartyAccountId);
        credit.ChartOfAccountId.Should().Be(InventoryTestIds.Branch1CoaId);
        debit.Amount.Should().Be(totalCost);
    }

    private static void AssertReceiptGlLines(Entry entry, decimal totalAmount)
    {
        var debit = entry.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Debit);
        var credit = entry.FinancialTransactions.Single(t => t.AccountNature == AccountNature.Credit);

        debit.ChartOfAccountId.Should().Be(InventoryTestIds.Branch1CoaId);
        credit.ChartOfAccountId.Should().Be(InventoryTestIds.PartyAccountId);
        debit.Amount.Should().Be(totalAmount);
        credit.Amount.Should().Be(totalAmount);
        debit.ComplementTransactionId.Should().Be(credit.Id);
        credit.ComplementTransactionId.Should().Be(debit.Id);
    }
}
