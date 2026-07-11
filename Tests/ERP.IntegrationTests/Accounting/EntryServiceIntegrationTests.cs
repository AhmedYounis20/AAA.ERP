using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ERP.IntegrationTests.Accounting;

public class EntryServiceIntegrationTests : IAsyncLifetime
{
    private IntegrationTestWebApplicationFactory _factory = null!;
    private AsyncServiceScope _scope;

    public async Task InitializeAsync()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _scope = _factory.Services.CreateAsyncScope();
        await InventoryTestDataSeeder.SeedAsync(_scope.ServiceProvider);
        await SeedEntriesAsync(_scope.ServiceProvider);
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task GetEntryNumber_with_no_matching_period_returns_bad_request()
    {
        var entryService = _scope.ServiceProvider.GetRequiredService<IEntryService>();

        var response = await entryService.GetEntryNumber(new DateTime(2099, 1, 1));

        response.IsSuccess.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Errors.Should().Contain(e => e.MessageKey == "NotFoundCurrentFinancialPeriod");
    }

    [Fact]
    public async Task GetPaginated_filters_by_entry_type()
    {
        var entryService = _scope.ServiceProvider.GetRequiredService<IEntryService>();

        var response = await entryService.GetPaginated(new EntryFilterDto
        {
            EntryType = (int)EntryType.Journal,
            PageNumber = 1,
            PageSize = 50
        });

        response.IsSuccess.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Result!.Items.Should().NotBeEmpty();
        response.Result.Items.Should().OnlyContain(e => e.EntryType == EntryType.Journal);
    }

    private static async Task SeedEntriesAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        if (await db.Set<Entry>().AnyAsync(e => e.Id == EntryTestIds.JournalEntryId))
            return;

        db.Set<Entry>().AddRange(
            new Entry
            {
                Id = EntryTestIds.JournalEntryId,
                EntryType = EntryType.Journal,
                EntryNumber = "1",
                EntryDate = InventoryTestIds.TransactionDate,
                BranchId = InventoryTestIds.Branch1Id,
                CurrencyId = InventoryTestIds.CurrencyId,
                FinancialPeriodId = InventoryTestIds.FinancialPeriodId,
                ExchangeRate = 1
            },
            new Entry
            {
                Id = EntryTestIds.PaymentEntryId,
                EntryType = EntryType.Payment,
                EntryNumber = "2",
                EntryDate = InventoryTestIds.TransactionDate,
                BranchId = InventoryTestIds.Branch1Id,
                CurrencyId = InventoryTestIds.CurrencyId,
                FinancialPeriodId = InventoryTestIds.FinancialPeriodId,
                ExchangeRate = 1
            });

        await db.SaveChangesAsync();
    }
}

internal static class EntryTestIds
{
    public static readonly Guid JournalEntryId = Guid.Parse("43000000-0000-0000-0000-000000000001");
    public static readonly Guid PaymentEntryId = Guid.Parse("43000000-0000-0000-0000-000000000002");
}
