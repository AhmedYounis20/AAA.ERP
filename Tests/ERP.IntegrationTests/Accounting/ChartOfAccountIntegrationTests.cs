using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Infrastructure.DBConfiguration.DbContext;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.IntegrationTests.Accounting;

public class ChartOfAccountIntegrationTests : IAsyncLifetime
{
    private IntegrationTestWebApplicationFactory _factory = null!;
    private AsyncServiceScope _scope;

    public async Task InitializeAsync()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _scope = _factory.Services.CreateAsyncScope();
        await SeedAsync(_scope.ServiceProvider);
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Create_chart_of_account_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<IChartOfAccountRepository>();

        var command = new ChartOfAccountCreateCommand
        {
            Name = "Cash Account",
            NameSecondLanguage = "Cash Account",
            Code = "8100",
            NodeType = NodeType.Domain,
            AccountGuidId = ChartOfAccountTestIds.AccountGuideId,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            IsDepreciable = false
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();
        response.Result!.Code.Should().Be("8100");

        var persisted = await repository.Get(response.Result.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("Cash Account");
    }

    [Fact]
    public async Task Create_chart_of_account_with_duplicate_code_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var command = new ChartOfAccountCreateCommand
        {
            Name = "Duplicate Code Account",
            NameSecondLanguage = "Duplicate Code Account",
            Code = ChartOfAccountTestIds.ExistingCode,
            NodeType = NodeType.Domain,
            AccountGuidId = ChartOfAccountTestIds.AccountGuideId,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "ChartOfAccoutWithSameCodeExist");
    }

    [Fact]
    public async Task Delete_chart_of_account_in_use_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new ChartOfAccountDeleteCommand(ChartOfAccountTestIds.InUseAccountId));

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "ChartOfAccountIsInUse");
    }

    [Fact]
    public async Task Update_account_nature_when_in_use_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<IChartOfAccountRepository>();

        var account = await repository.Get(ChartOfAccountTestIds.InUseAccountId);
        account.Should().NotBeNull();

        var command = new ChartOfAccountUpdateCommand
        {
            Id = account!.Id,
            Name = account.Name!,
            NameSecondLanguage = account.NameSecondLanguage!,
            Code = account.Code!,
            NodeType = account.NodeType,
            AccountGuidId = account.AccountGuidId,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = account.IsActiveAccount,
            IsPostedAccount = account.IsPostedAccount,
            IsStopDealing = account.IsStopDealing,
            IsDepreciable = account.IsDepreciable
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "ChartOfAccountNatureCannotChange");
    }

    [Fact]
    public async Task Delete_account_guide_in_use_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new AccountGuideDeleteCommand(ChartOfAccountTestIds.AccountGuideId));

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "AccountGuideIsUsedInChartOfAccounts");
    }

    [Fact]
    public async Task GetPaginated_filters_by_account_nature()
    {
        var queries = _scope.ServiceProvider.GetRequiredService<IChartOfAccountQueries>();

        var response = await queries.GetPaginated(new ChartOfAccountFilterDto
        {
            AccountNature = (int)AccountNature.Credit,
            PageNumber = 1,
            PageSize = 50
        });

        response.IsSuccess.Should().BeTrue();
        response.Result.Should().NotBeNull();
        response.Result!.Items.Should().NotBeEmpty();
        response.Result.Items.Should().OnlyContain(e => e.AccountNature == AccountNature.Credit);
    }

    private static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Set<AccountGuide>().AnyAsync(e => e.Id == ChartOfAccountTestIds.AccountGuideId))
            return;

        db.Set<AccountGuide>().Add(new AccountGuide
        {
            Id = ChartOfAccountTestIds.AccountGuideId,
            Name = "COA Test Guide",
            NameSecondLanguage = "COA Test Guide"
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = ChartOfAccountTestIds.ExistingAccountId,
            Name = "Existing Account",
            NameSecondLanguage = "Existing Account",
            Code = ChartOfAccountTestIds.ExistingCode,
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = ChartOfAccountTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = ChartOfAccountTestIds.InUseAccountId,
            Name = "In Use Account",
            NameSecondLanguage = "In Use Account",
            Code = "8200",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = ChartOfAccountTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = ChartOfAccountTestIds.CreditAccountId,
            Name = "Credit Filter Account",
            NameSecondLanguage = "Credit Filter Account",
            Code = "8300",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = ChartOfAccountTestIds.AccountGuideId
        });

        db.Set<Tax>().Add(new Tax
        {
            Id = ChartOfAccountTestIds.TaxId,
            Name = "COA Usage Tax",
            NameSecondLanguage = "COA Usage Tax",
            NodeType = NodeType.Domain,
            ChartOfAccountId = ChartOfAccountTestIds.InUseAccountId,
            Percent = 5m
        });

        await db.SaveChangesAsync();
    }
}

internal static class ChartOfAccountTestIds
{
    public static readonly Guid AccountGuideId = Guid.Parse("41000000-0000-0000-0000-000000000000");
    public static readonly Guid ExistingAccountId = Guid.Parse("41000000-0000-0000-0000-000000000001");
    public static readonly Guid InUseAccountId = Guid.Parse("41000000-0000-0000-0000-000000000002");
    public static readonly Guid CreditAccountId = Guid.Parse("41000000-0000-0000-0000-000000000003");
    public static readonly Guid TaxId = Guid.Parse("41000000-0000-0000-0000-000000000004");
    public const string ExistingCode = "8000";
}
