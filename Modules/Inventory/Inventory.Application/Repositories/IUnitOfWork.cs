using Inventory.Domain.Models.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Repositories;
using Shared.Domain.BaseEntities;

namespace Inventory.Application.Repositories;

public interface IUnitOfWork
{
    public ISizeRepository SizeRepository { get; }
    public IColorRepository ColorRepository { get; }
    public IPackingUnitRepository PackingUnitRepository { get; }
    public IManufacturerCompanyRepository ManufacturerCompanyRepository { get; }
    public ISellingPriceRepository SellingPriceRepository { get; }
    public IStockBalanceRepository StockBalanceRepository { get; }
    public IInventoryTransactionRepository InventoryTransactionRepository { get; }
    public IAttributeDefinitionRepository AttributeDefinitionRepository { get; }
    public IAttributeValueRepository AttributeValueRepository { get; }
    public IAttachmentRepository AttachmentRepository { get; }
    public IBaseRepository<Item> ItemRepository { get; }
    public IBaseRepository<ItemSupplier> ItemSupplierRepository { get; }
    public IBaseRepository<ItemCode> ItemCodeRepository { get; }
    public IBaseRepository<ItemManufacturerCompany> ItemManufacturerCompanyRepository { get; }
    public IBaseRepository<ItemPackingUnit> ItemPackingUnitRepository { get; }
    public IBaseRepository<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPriceRepository { get; }
    public IBaseRepository<ItemSellingPriceDiscount> ItemSellingPriceDiscountRepository { get; }
    public DbSet<T> Set<T>() where T : BaseEntity;
    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
    public Task<int> SaveChanges();
}