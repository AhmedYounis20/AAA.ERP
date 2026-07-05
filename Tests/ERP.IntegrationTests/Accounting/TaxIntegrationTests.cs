using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Domain.Entities;
using ERP.Infrastructure.DBConfiguration.DbContext;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.BaseEntities;

namespace ERP.IntegrationTests.Accounting;

public class TaxIntegrationTests : IAsyncLifetime
{
    private IntegrationTestWebApplicationFactory _factory = null!;
    private AsyncServiceScope _scope;

    public async Task InitializeAsync()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _scope = _factory.Services.CreateAsyncScope();
        await SeedTaxParentChartOfAccountAsync(_scope.ServiceProvider);
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Create_tax_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<ITaxRepository>();

        var command = new TaxCreateCommand
        {
            Name = "VAT 14%",
            NameSecondLanguage = "VAT 14%",
            NodeType = NodeType.Domain,
            ChartOfAccountId = TaxTestIds.ParentChartOfAccountId,
            Percent = 14m
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();
        response.Result!.Percent.Should().Be(14m);

        var persisted = await repository.Get(response.Result.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("VAT 14%");
        persisted.Percent.Should().Be(14m);
    }

    private static async Task SeedTaxParentChartOfAccountAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Set<ChartOfAccount>().AnyAsync(e => e.Id == TaxTestIds.ParentChartOfAccountId))
            return;

        db.Set<AccountGuide>().Add(new AccountGuide
        {
            Id = TaxTestIds.AccountGuideId,
            Name = "Tax Test Account Guide",
            NameSecondLanguage = "Tax Test Account Guide"
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = TaxTestIds.ParentChartOfAccountId,
            Name = "Tax Parent",
            NameSecondLanguage = "Tax Parent",
            Code = "9000",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = TaxTestIds.AccountGuideId
        });

        await db.SaveChangesAsync();
    }
}

internal static class TaxTestIds
{
    public static readonly Guid AccountGuideId = Guid.Parse("40000000-0000-0000-0000-000000000000");
    public static readonly Guid ParentChartOfAccountId = Guid.Parse("40000000-0000-0000-0000-000000000001");
}
