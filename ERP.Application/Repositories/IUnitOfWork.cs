using ERP.Application.Repositories.BaseRepositories;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Inventory.ExpiryLevels;
using ERP.Domain.Models.Entities.Inventory.InventoryThresholds;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
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
    public IAttributeValueRepository AttributeValueRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IBaseRepository<ProductSupplier> ProductSupplierRepository { get; }
    public IBaseRepository<ProductSalesTax> ProductSalesTaxRepository { get; }
    public IBaseRepository<ProductPurchaseTax> ProductPurchaseTaxRepository { get; }
    public IBaseRepository<ProductCostCenter> ProductCostCenterRepository { get; }
    public IBaseRepository<ProductCode> ProductCodeRepository { get; }
    public IBaseRepository<ProductAttachment> ProductAttachmentRepository { get; }
    public IBaseRepository<ProductManufacturerCompany> ProductManufacturerCompanyRepository { get; }
    public IBaseRepository<ProductPackingUnit> ProductPackingUnitRepository { get; }
    public IBaseRepository<ProductPackingUnitSellingPrice> ProductPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<ProductSellingPriceDiscount> ProductSellingPriceDiscountRepository { get; }
    public IProductAttributeDefinitionRepository ProductAttributeDefinitionRepository { get; }
    public IVariantRepository VariantRepository { get; }
    public IBaseRepository<VariantSupplier> VariantSupplierRepository { get; }
    public IBaseRepository<VariantSalesTax> VariantSalesTaxRepository { get; }
    public IBaseRepository<VariantPurchaseTax> VariantPurchaseTaxRepository { get; }
    public IBaseRepository<VariantCostCenter> VariantCostCenterRepository { get; }
    public IBaseRepository<VariantCode> VariantCodeRepository { get; }
    public IBaseRepository<VariantAttachment> VariantAttachmentRepository { get; }
    public IBaseRepository<VariantManufacturerCompany> VariantManufacturerCompanyRepository { get; }
    public IBaseRepository<VariantPackingUnit> VariantPackingUnitRepository { get; }
    public IBaseRepository<VariantPackingUnitSellingPrice> VariantPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<VariantSellingPriceDiscount> VariantSellingPriceDiscountRepository { get; }
    public IVariantAttributeValueRepository VariantAttributeValueRepository { get; }
    public IInventoryTransactionRepository InventoryTransactionRepository { get; }
    public IInventoryTransferRepository InventoryTransferRepository { get; }
    public IStockBalanceRepository StockBalanceRepository { get; }
    public IEntryRepository EntryRepository { get; }
    public IBaseRepository<FinancialTransaction> FinancialTransactionRepository { get; }
    public IBaseRepository<EntryAttachment> EntryAttachmentRepository { get; }
    public IBaseRepository<InventoryThreshold> InventoryThresholdRepository { get; }
    public IBaseRepository<ExpiryLevel> ExpiryLevelRepository { get; }
    public DbSet<T> Set<T>() where T : BaseEntity;
    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
    public Task<int> SaveChanges();
}