using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Application.Repositories;

public interface IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get;  }
    public IChartOfAccountRepository ChartOfAccountRepository { get; }
    public ICurrencyRepository CurrencyRepository { get; }
    public IGLSettingRepository GlSettingRepository { get; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; }
    public ICashInBoxRepository CashInBoxRepository { get;  }
    public IAttachmentRepository AttachmentRepository { get; }
    public IBranchRepository BranchRepository { get; }
    public IPackingUnitRepository PackingUnitRepository { get; }
    public IItemRepository ItemRepository { get; }
    public IBaseRepository<ItemSupplier> ItemSupplierRepository { get; }
    public IBaseRepository<ItemCode> ItemCodeRepository { get; }
    public IBaseRepository<ItemManufacturerCompany> ItemManufacturerCompanyRepository { get; }
    public IBaseRepository<ItemPackingUnit> ItemPackingUnitRepository { get; }
    public IBaseRepository<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<ItemSellingPriceDiscount> ItemSellingPriceDiscountRepository { get; }

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}