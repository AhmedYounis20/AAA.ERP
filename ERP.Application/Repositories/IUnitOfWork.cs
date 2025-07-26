using ERP.Application.Repositories.BaseRepositories;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Inventory.Items;
using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;

namespace ERP.Application.Repositories;

public interface IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get; }
    public ISizeRepository SizeRepository { get; }
    public IColorRepository ColorRepository { get; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; }
    public ICurrencyRepository CurrencyRepository { get; }
    public IGLSettingRepository GlSettingRepository { get; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; }
    public ICashInBoxRepository CashInBoxRepository { get; }
    public IAttachmentRepository AttachmentRepository { get; }
    public IBranchRepository BranchRepository { get; }
    public ISupplierRepository SupplierRepository { get; }
    public IPackingUnitRepository PackingUnitRepository { get; }
    public IItemRepository ItemRepository { get; }
    public IBaseRepository<ItemSupplier> ItemSupplierRepository { get; }
    public IBaseRepository<ItemCode> ItemCodeRepository { get; }
    public IBaseRepository<ItemManufacturerCompany> ItemManufacturerCompanyRepository { get; }
    public IBaseRepository<ItemPackingUnit> ItemPackingUnitRepository { get; }
    public IBaseRepository<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<ItemSellingPriceDiscount> ItemSellingPriceDiscountRepository { get; }
    public IInventoryTransactionRepository InventoryTransactionRepository { get; }
    public IInventoryTransferRepository InventoryTransferRepository { get; }
    public IStockBalanceRepository StockBalanceRepository { get; }
    public IEntryRepository EntryRepository { get; }
    public IBaseRepository<FinancialTransaction> FinancialTransactionRepository { get; }
    public IBaseRepository<EntryAttachment> EntryAttachmentRepository { get; }
    public DbSet<T> Set<T>() where T : BaseEntity;
    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
    public Task<int> SaveChanges();
}