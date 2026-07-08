using Accounting.Application.Commands.Banks;
using Accounting.Application.Commands.Branches;
using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Commands.Customers;
using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Commands.Suppliers;
using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.AccountGuides;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using ERP.Infrastructure.DBConfiguration.DbContext;
using ERP.IntegrationTests.Infrastructure;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Domain.Entities;
using System.Net;

namespace ERP.IntegrationTests.Accounting;

public class SubLedgerIntegrationTests : IAsyncLifetime
{
    private IntegrationTestWebApplicationFactory _factory = null!;
    private AsyncServiceScope _scope;

    public async Task InitializeAsync()
    {
        _factory = new IntegrationTestWebApplicationFactory();
        _scope = _factory.Services.CreateAsyncScope();
        await SeedSubLedgerParentsAsync(_scope.ServiceProvider);
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task Create_bank_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<IBankRepository>();

        var command = new BankCreateCommand
        {
            Name = "National Bank",
            NameSecondLanguage = "National Bank",
            NodeType = NodeType.Domain,
            Email = "info@bank.test",
            Phone = "01000000000"
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Result.Should().NotBeNull();
        response.Result!.ChartOfAccountId.Should().NotBeNull();

        var persisted = await repository.Get(response.Result.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("National Bank");
    }

    [Fact]
    public async Task Create_bank_with_duplicate_name_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var command = new BankCreateCommand
        {
            Name = SubLedgerTestIds.ExistingBankName,
            NameSecondLanguage = SubLedgerTestIds.ExistingBankName,
            NodeType = NodeType.Domain
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "WithSameNameIsExisted");
    }

    [Fact]
    public async Task Delete_bank_not_found_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new BankDeleteCommand(Guid.Parse("42000000-0000-0000-0000-000000009999")));

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "RecordNotFound");
    }

    [Fact]
    public async Task Create_branch_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<IBranchRepository>();

        var command = new BranchCreateCommand
        {
            Name = "Main Branch",
            NameSecondLanguage = "Main Branch",
            NodeType = NodeType.Domain,
            Address = "HQ"
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();

        var persisted = await repository.Get(response.Result!.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("Main Branch");
    }

    [Fact]
    public async Task Create_customer_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        var command = new CustomerCreateCommand
        {
            Name = "ACME Customer",
            NameSecondLanguage = "ACME Customer",
            NodeType = NodeType.Domain,
            CustomerType = CustomerType.Consumer
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();

        var persisted = await repository.Get(response.Result!.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("ACME Customer");
    }

    [Fact]
    public async Task Create_supplier_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

        var command = new SupplierCreateCommand
        {
            Name = "ACME Supplier",
            NameSecondLanguage = "ACME Supplier",
            NodeType = NodeType.Domain,
            CustomerType = CustomerType.WholeSale
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();

        var persisted = await repository.Get(response.Result!.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("ACME Supplier");
    }

    [Fact]
    public async Task Create_cash_in_box_via_MediatR_persists_and_can_be_read_back()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<ICashInBoxRepository>();

        var command = new CashInBoxCreateCommand
        {
            Name = "Main Cash Box",
            NameSecondLanguage = "Main Cash Box",
            NodeType = NodeType.Domain
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();

        var persisted = await repository.Get(response.Result!.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("Main Cash Box");
    }

    [Fact]
    public async Task Create_fixed_asset_with_non_email_model_succeeds()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<IFixedAssetRepository>();

        var command = new FixedAssetCreateCommand
        {
            Name = "Office Laptop",
            NameSecondLanguage = "Office Laptop",
            NodeType = NodeType.Domain,
            FixedAssetType = FixedAssetType.ElectronicDevices,
            Model = "Dell Latitude 7420",
            Serial = "SN-12345",
            IsDepreciable = false
        };

        var response = await sender.Send(command);

        response.IsSuccess.Should().BeTrue(response.Errors?.FirstOrDefault()?.MessageKey);
        response.Result.Should().NotBeNull();
        response.Result!.Model.Should().Be("Dell Latitude 7420");

        var persisted = await repository.Get(response.Result.Id);
        persisted.Should().NotBeNull();
        persisted!.Model.Should().Be("Dell Latitude 7420");
    }

    [Fact]
    public async Task Update_fixed_asset_with_duplicate_name_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var firstCreate = await sender.Send(new FixedAssetCreateCommand
        {
            Name = "Fixed Asset A",
            NameSecondLanguage = "Fixed Asset A",
            NodeType = NodeType.Domain,
            FixedAssetType = FixedAssetType.ElectronicDevices,
            IsDepreciable = false
        });

        var secondCreate = await sender.Send(new FixedAssetCreateCommand
        {
            Name = "Fixed Asset B",
            NameSecondLanguage = "Fixed Asset B",
            NodeType = NodeType.Domain,
            FixedAssetType = FixedAssetType.ElectronicDevices,
            IsDepreciable = false
        });

        firstCreate.IsSuccess.Should().BeTrue(firstCreate.Errors?.FirstOrDefault()?.MessageKey);
        secondCreate.IsSuccess.Should().BeTrue(secondCreate.Errors?.FirstOrDefault()?.MessageKey);

        var update = new FixedAssetUpdateCommand
        {
            Id = secondCreate.Result!.Id,
            Name = "Fixed Asset A",
            NameSecondLanguage = "Fixed Asset A",
            NodeType = NodeType.Domain,
            IsDepreciable = false
        };

        var response = await sender.Send(update);

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "WithSameNameIsExisted");
    }

    [Fact]
    public async Task Delete_fixed_asset_not_found_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new FixedAssetDeleteCommand(Guid.Parse("42000000-0000-0000-0000-000000009998")));

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "RecordNotFound");
    }

    [Fact]
    public async Task Create_and_update_tax_via_MediatR_persists_changes()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        var repository = _scope.ServiceProvider.GetRequiredService<ITaxRepository>();
        var parentTaxAccountId = Guid.Parse(SD.CashInBoxChartOfAccountId);

        var create = await sender.Send(new TaxCreateCommand
        {
            Name = "VAT 14",
            NameSecondLanguage = "VAT 14",
            NodeType = NodeType.Domain,
            ChartOfAccountId = parentTaxAccountId,
            Percent = 14m
        });

        create.IsSuccess.Should().BeTrue(create.Errors?.FirstOrDefault()?.MessageKey);
        create.Result.Should().NotBeNull();

        var db = _scope.ServiceProvider.GetRequiredService<IntegrationTestDbContext>();
        db.ChangeTracker.Clear();

        var update = await sender.Send(new TaxUpdateCommand
        {
            Id = create.Result!.Id,
            Name = "VAT 15",
            NameSecondLanguage = "VAT 15",
            NodeType = NodeType.Domain,
            Percent = 15m
        });

        update.IsSuccess.Should().BeTrue(update.Errors?.FirstOrDefault()?.MessageKey);

        var persisted = await repository.Get(create.Result.Id);
        persisted.Should().NotBeNull();
        persisted!.Name.Should().Be("VAT 15");
        persisted.Percent.Should().Be(15m);
    }

    [Fact]
    public async Task Delete_tax_not_found_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new TaxDeleteCommand(Guid.Parse("42000000-0000-0000-0000-000000009996")));

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "RecordNotFound");
    }

    [Fact]
    public async Task Update_fixed_asset_not_found_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var response = await sender.Send(new FixedAssetUpdateCommand
        {
            Id = Guid.Parse("42000000-0000-0000-0000-000000009997"),
            Name = "Missing Asset",
            NameSecondLanguage = "Missing Asset",
            NodeType = NodeType.Domain,
            IsDepreciable = false
        });

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "RecordNotFound");
    }

    [Fact]
    public async Task Update_fixed_asset_node_type_change_fails()
    {
        var sender = _scope.ServiceProvider.GetRequiredService<ISender>();

        var create = await sender.Send(new FixedAssetCreateCommand
        {
            Name = "Immutable Asset",
            NameSecondLanguage = "Immutable Asset",
            NodeType = NodeType.Domain,
            FixedAssetType = FixedAssetType.ElectronicDevices,
            IsDepreciable = false
        });

        create.IsSuccess.Should().BeTrue(create.Errors?.FirstOrDefault()?.MessageKey);

        var response = await sender.Send(new FixedAssetUpdateCommand
        {
            Id = create.Result!.Id,
            Name = "Immutable Asset Updated",
            NameSecondLanguage = "Immutable Asset Updated",
            NodeType = NodeType.Category,
            IsDepreciable = false
        });

        response.IsSuccess.Should().BeFalse();
        response.Errors.Should().Contain(e => e.MessageKey == "CannotChangeNodeType");
    }

    private static async Task SeedSubLedgerParentsAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<IntegrationTestDbContext>();
        await db.Database.EnsureCreatedAsync();

        if (await db.Set<AccountGuide>().AnyAsync(e => e.Id == SubLedgerTestIds.AccountGuideId))
            return;

        db.Set<AccountGuide>().Add(new AccountGuide
        {
            Id = SubLedgerTestIds.AccountGuideId,
            Name = "SubLedger Test Guide",
            NameSecondLanguage = "SubLedger Test Guide"
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.BankChartAccountId),
            Name = "Banks Parent",
            NameSecondLanguage = "Banks Parent",
            Code = "1100",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.BranchChartOfAccountId),
            Name = "Branches Parent",
            NameSecondLanguage = "Branches Parent",
            Code = "1200",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.CustomerChartOfAccountId),
            Name = "Customers Parent",
            NameSecondLanguage = "Customers Parent",
            Code = "1300",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.SupplierChartOfAccountId),
            Name = "Suppliers Parent",
            NameSecondLanguage = "Suppliers Parent",
            Code = "1400",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.CashInBoxChartOfAccountId),
            Name = "Cash Boxes Parent",
            NameSecondLanguage = "Cash Boxes Parent",
            Code = "1500",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.FixedAssetsIds[FixedAssetType.ElectronicDevices]),
            Name = "Electronic Devices Parent",
            NameSecondLanguage = "Electronic Devices Parent",
            Code = "1504",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.AccumlatedDepreciationId),
            Name = "Accumulated Depreciation",
            NameSecondLanguage = "Accumulated Depreciation",
            Code = "1600",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.ExpensesDepreciationId),
            Name = "Depreciation Expense",
            NameSecondLanguage = "Depreciation Expense",
            Code = "5100",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.FixedAssetsDepreciationDetails[FixedAssetType.ElectronicDevices][0]),
            Name = "Electronic Devices Accumulated Depreciation",
            NameSecondLanguage = "Electronic Devices Accumulated Depreciation",
            Code = "1504-AD",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = Guid.Parse(SD.FixedAssetsDepreciationDetails[FixedAssetType.ElectronicDevices][1]),
            Name = "Electronic Devices Depreciation Expense",
            NameSecondLanguage = "Electronic Devices Depreciation Expense",
            Code = "5104",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Debit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<Bank>().Add(new Bank
        {
            Id = SubLedgerTestIds.ExistingBankId,
            Name = SubLedgerTestIds.ExistingBankName,
            NameSecondLanguage = SubLedgerTestIds.ExistingBankName,
            NodeType = NodeType.Domain
        });

        await db.SaveChangesAsync();
    }
}

internal static class SubLedgerTestIds
{
    public static readonly Guid AccountGuideId = Guid.Parse("42000000-0000-0000-0000-000000000000");
    public static readonly Guid ExistingBankId = Guid.Parse("42000000-0000-0000-0000-000000000001");
    public const string ExistingBankName = "Seeded Bank";
}
