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
        var parentTaxAccountId = SubLedgerTestIds.TaxParentAccountId;

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

        AddSubLedgerBaseAccount(db, SubLedgerTestIds.BankParentId, "Banks Parent", "1100", AccountNature.Debit, SubLeadgerType.Bank);
        AddSubLedgerBaseAccount(db, SubLedgerTestIds.BranchParentId, "Branches Parent", "1200", AccountNature.Debit, SubLeadgerType.Branch);
        AddSubLedgerBaseAccount(db, SubLedgerTestIds.CustomerParentId, "Customers Parent", "1300", AccountNature.Debit, SubLeadgerType.Customer);
        AddSubLedgerBaseAccount(db, SubLedgerTestIds.SupplierParentId, "Suppliers Parent", "1400", AccountNature.Credit, SubLeadgerType.Supplier);
        AddSubLedgerBaseAccount(db, SubLedgerTestIds.CashInBoxParentId, "Cash Boxes Parent", "1500", AccountNature.Debit, SubLeadgerType.CashInBox);

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = SubLedgerTestIds.TaxParentAccountId,
            Name = "Tax Parent",
            NameSecondLanguage = "Tax Parent",
            Code = "1550",
            NodeType = NodeType.Domain,
            AccountNature = AccountNature.Credit,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        AddSubLedgerBaseAccount(
            db,
            SubLedgerTestIds.FixedAssetElectronicDevicesAssetId,
            "Electronic Devices Parent",
            "1504",
            AccountNature.Debit,
            SubLeadgerType.FixedAsset,
            FixedAssetType.ElectronicDevices,
            SubLedgerAccountRole.Asset);

        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = SubLedgerTestIds.AccumulatedDepreciationHeaderId,
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
            Id = SubLedgerTestIds.ExpensesDepreciationHeaderId,
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

        AddSubLedgerBaseAccount(
            db,
            SubLedgerTestIds.FixedAssetElectronicDevicesAccumId,
            "Electronic Devices Accumulated Depreciation",
            "1504-AD",
            AccountNature.Credit,
            SubLeadgerType.FixedAsset,
            FixedAssetType.ElectronicDevices,
            SubLedgerAccountRole.AccumulatedDepreciation,
            SubLedgerTestIds.AccumulatedDepreciationHeaderId);

        AddSubLedgerBaseAccount(
            db,
            SubLedgerTestIds.FixedAssetElectronicDevicesExpenseId,
            "Electronic Devices Depreciation Expense",
            "5104",
            AccountNature.Debit,
            SubLeadgerType.FixedAsset,
            FixedAssetType.ElectronicDevices,
            SubLedgerAccountRole.DepreciationExpense,
            SubLedgerTestIds.ExpensesDepreciationHeaderId);

        db.Set<Bank>().Add(new Bank
        {
            Id = SubLedgerTestIds.ExistingBankId,
            Name = SubLedgerTestIds.ExistingBankName,
            NameSecondLanguage = SubLedgerTestIds.ExistingBankName,
            NodeType = NodeType.Domain
        });

        await db.SaveChangesAsync();
    }

    private static void AddSubLedgerBaseAccount(
        IntegrationTestDbContext db,
        Guid id,
        string name,
        string code,
        AccountNature nature,
        SubLeadgerType subLedgerType,
        FixedAssetType? fixedAssetType = null,
        SubLedgerAccountRole role = SubLedgerAccountRole.Default,
        Guid? parentId = null)
    {
        db.Set<ChartOfAccount>().Add(new ChartOfAccount
        {
            Id = id,
            Name = name,
            NameSecondLanguage = name,
            Code = code,
            ParentId = parentId,
            NodeType = NodeType.Domain,
            AccountNature = nature,
            IsActiveAccount = true,
            IsPostedAccount = false,
            IsStopDealing = false,
            SubLeadgerType = subLedgerType,
            AccountGuidId = SubLedgerTestIds.AccountGuideId
        });

        db.Set<SubLedgerBaseAccount>().Add(
            SubLedgerBaseAccount.Create(id, subLedgerType, role, fixedAssetType));
    }
}

internal static class SubLedgerTestIds
{
    public static readonly Guid AccountGuideId = Guid.Parse("42000000-0000-0000-0000-000000000000");
    public static readonly Guid ExistingBankId = Guid.Parse("42000000-0000-0000-0000-000000000001");
    public static readonly Guid BankParentId = Guid.Parse("42000000-0000-0000-0000-000000000010");
    public static readonly Guid BranchParentId = Guid.Parse("42000000-0000-0000-0000-000000000011");
    public static readonly Guid CustomerParentId = Guid.Parse("42000000-0000-0000-0000-000000000012");
    public static readonly Guid SupplierParentId = Guid.Parse("42000000-0000-0000-0000-000000000013");
    public static readonly Guid CashInBoxParentId = Guid.Parse("42000000-0000-0000-0000-000000000014");
    public static readonly Guid TaxParentAccountId = Guid.Parse("42000000-0000-0000-0000-000000000015");
    public static readonly Guid FixedAssetElectronicDevicesAssetId = Guid.Parse("42000000-0000-0000-0000-000000000016");
    public static readonly Guid AccumulatedDepreciationHeaderId = Guid.Parse("42000000-0000-0000-0000-000000000017");
    public static readonly Guid ExpensesDepreciationHeaderId = Guid.Parse("42000000-0000-0000-0000-000000000018");
    public static readonly Guid FixedAssetElectronicDevicesAccumId = Guid.Parse("42000000-0000-0000-0000-000000000019");
    public static readonly Guid FixedAssetElectronicDevicesExpenseId = Guid.Parse("42000000-0000-0000-0000-00000000001A");
    public const string ExistingBankName = "Seeded Bank";
}
