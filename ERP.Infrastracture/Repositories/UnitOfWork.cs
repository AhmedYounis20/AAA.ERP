using ERP.Application.Repositories.Account;
using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Repositories.Inventory;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Infrastracture.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationDbContext _context;
    private IDbContextTransaction? _sqlTransaction;

    public IAccountGuideRepository AccountGuideRepository { get; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; }
    public ICurrencyRepository CurrencyRepository { get; }
    public IGLSettingRepository GlSettingRepository { get; }
    public IEntryRepository EntryRepository { get; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; }
    public IAttachmentRepository AttachmentRepository { get; }
    public ICashInBoxRepository CashInBoxRepository { get; }
    public IBranchRepository BranchRepository { get; }
    public IPackingUnitRepository PackingUnitRepository { get; }
    public IItemRepository ItemRepository { get; }
    public IBaseRepository<ItemSupplier> ItemSupplierRepository { get; }
    public IBaseRepository<ItemCode> ItemCodeRepository { get; }
    public IBaseRepository<ItemManufacturerCompany> ItemManufacturerCompanyRepository { get; }
    public IBaseRepository<ItemPackingUnit> ItemPackingUnitRepository { get; }
    public ISizeRepository SizeRepository { get; }
    public IColorRepository ColorRepository { get; }
    public IBaseRepository<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<ItemSellingPriceDiscount> ItemSellingPriceDiscountRepository { get; }
    public ISupplierRepository SupplierRepository { get; }
    public IInventoryTransactionRepository InventoryTransactionRepository { get; }
    public IStockBalanceRepository StockBalanceRepository { get; }
    public IBaseRepository<EntryAttachment> EntryAttachmentRepository { get; }
    public IBaseRepository<FinancialTransaction> FinancialTransactionRepository { get; }
    public IInventoryTransferRepository InventoryTransferRepository { get; }

    public UnitOfWork(
        IApplicationDbContext context,
        IAccountGuideRepository accountGuideRepository,
        IChartOfAccountRepository chartOfAccountRepository,
        ICurrencyRepository currencyRepository,
        IGLSettingRepository glSettingRepository,
        IEntryRepository entryRepository,
        IFinancialPeriodRepository financialPeriodRepository,
        IAttachmentRepository attachmentRepository,
        ICashInBoxRepository cashInBoxRepository,
        IBranchRepository branchRepository,
        IPackingUnitRepository packingUnitRepository,
        IItemRepository itemRepository,
        IBaseRepository<ItemSupplier> itemSupplierRepository,
        IBaseRepository<ItemCode> itemCodeRepository,
        IBaseRepository<ItemManufacturerCompany> itemManufacturerCompanyRepository,
        IBaseRepository<ItemPackingUnit> itemPackingUnitRepository,
        IBaseRepository<ItemPackingUnitSellingPrice> itemPackingUnitSellingPriceRepository,
        IBaseRepository<ItemSellingPriceDiscount> itemSellingPriceDiscountRepository,
        ISupplierRepository supplierRepository,
        ISizeRepository sizeRepository,
        IColorRepository colorRepository,
        IInventoryTransactionRepository inventoryTransactionRepository,
        IStockBalanceRepository stockBalanceRepository,
        IBaseRepository<EntryAttachment> entryAttachmentRepository,
        IBaseRepository<FinancialTransaction> financialTransactionRepository,
        IInventoryTransferRepository inventoryTransferRepository
    )
    {
        _context = context;
        AccountGuideRepository = accountGuideRepository;
        ChartOfAccountRepository = chartOfAccountRepository;
        CurrencyRepository = currencyRepository;
        GlSettingRepository = glSettingRepository;
        EntryRepository = entryRepository;
        FinancialPeriodRepository = financialPeriodRepository   ;
        AttachmentRepository = attachmentRepository;
        CashInBoxRepository = cashInBoxRepository;
        BranchRepository = branchRepository;
        PackingUnitRepository = packingUnitRepository;
        ItemRepository = itemRepository;
        ItemSupplierRepository = itemSupplierRepository;
        ItemCodeRepository = itemCodeRepository;
        ItemManufacturerCompanyRepository = itemManufacturerCompanyRepository;
        ItemPackingUnitRepository = itemPackingUnitRepository;
        ItemPackingUnitSellingPriceRepository = itemPackingUnitSellingPriceRepository;
        ItemSellingPriceDiscountRepository = itemSellingPriceDiscountRepository;
        SupplierRepository = supplierRepository;
        SizeRepository = sizeRepository;
        ColorRepository = colorRepository;
        InventoryTransactionRepository = inventoryTransactionRepository;
        StockBalanceRepository = stockBalanceRepository;
        EntryAttachmentRepository = entryAttachmentRepository;
        FinancialTransactionRepository = financialTransactionRepository;
        InventoryTransferRepository = inventoryTransferRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _sqlTransaction ??=  await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_sqlTransaction is not null)
        {
            await _sqlTransaction.CommitAsync();
            await _sqlTransaction.DisposeAsync();
            _sqlTransaction = null;
        }
    }

    public DbSet<T> Set<T>() where T : BaseEntity
    {
        return _context.Set<T>();
    }
    public  async Task<int> SaveChanges() 
    {
        return await _context.SaveChangesAsync();
    }
    public async Task RollbackAsync()
    {
        if (_sqlTransaction is not null)
        {
            await _sqlTransaction.RollbackAsync();
            await _sqlTransaction.DisposeAsync();
            _sqlTransaction = null;
        }
    }
}
