using ERP.Application.Data;
using ERP.Application.Repositories.Inventory;
using Inventory.Domain.Models.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Application.Repositories;
using Shared.Domain.BaseEntities;

namespace Inventory.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IInventoryDbContext _context;
    private IDbContextTransaction? _sqlTransaction;

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
    public IInventoryTransactionRepository InventoryTransactionRepository { get; }
    public IStockBalanceRepository StockBalanceRepository { get; }
    public IAttachmentRepository AttachmentRepository { get; }
    public IInventoryTransferRepository InventoryTransferRepository { get; }

    public IManufacturerCompanyRepository ManufacturerCompanyRepository { get; }
    public ISellingPriceRepository SellingPriceRepository { get; }
    public IAttributeDefinitionRepository AttributeDefinitionRepository { get; }
    public IAttributeValueRepository AttributeValueRepository { get; }

    IBaseRepository<Item> IUnitOfWork.ItemRepository => ItemRepository;

    public UnitOfWork(
        IInventoryDbContext context,
        IPackingUnitRepository packingUnitRepository,
        IItemRepository itemRepository,
        IBaseRepository<ItemSupplier> itemSupplierRepository,
        IBaseRepository<ItemCode> itemCodeRepository,
        IBaseRepository<ItemManufacturerCompany> itemManufacturerCompanyRepository,
        IBaseRepository<ItemPackingUnit> itemPackingUnitRepository,
        IBaseRepository<ItemPackingUnitSellingPrice> itemPackingUnitSellingPriceRepository,
        IBaseRepository<ItemSellingPriceDiscount> itemSellingPriceDiscountRepository,
        ISizeRepository sizeRepository,
        IColorRepository colorRepository,
        IInventoryTransactionRepository inventoryTransactionRepository,
        IStockBalanceRepository stockBalanceRepository,
        IInventoryTransferRepository inventoryTransferRepository,
        IManufacturerCompanyRepository manufacturerCompanyRepository,
        ISellingPriceRepository sellingPriceRepository,
        IAttributeDefinitionRepository attributeDefinitionRepository,
        IAttributeValueRepository attributeValueRepository,
        IAttachmentRepository attachmentRepository

    )
    {
        _context = context;
        PackingUnitRepository = packingUnitRepository;
        ItemRepository = itemRepository;
        ItemSupplierRepository = itemSupplierRepository;
        ItemCodeRepository = itemCodeRepository;
        ItemManufacturerCompanyRepository = itemManufacturerCompanyRepository;
        ItemPackingUnitRepository = itemPackingUnitRepository;
        ItemPackingUnitSellingPriceRepository = itemPackingUnitSellingPriceRepository;
        ItemSellingPriceDiscountRepository = itemSellingPriceDiscountRepository;
        SizeRepository = sizeRepository;
        ColorRepository = colorRepository;
        InventoryTransactionRepository = inventoryTransactionRepository;
        StockBalanceRepository = stockBalanceRepository;
        InventoryTransferRepository = inventoryTransferRepository;
        ManufacturerCompanyRepository = manufacturerCompanyRepository;
        SellingPriceRepository = sellingPriceRepository;
        AttributeDefinitionRepository = attributeDefinitionRepository;
        AttributeValueRepository = attributeValueRepository;
        AttachmentRepository = attachmentRepository;
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
