using Inventory.Application.Repositories;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Domain.Models.Entities.Variants;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class SellingPriceRepository : BaseSettingRepository<SellingPrice>, ISellingPriceRepository
{
    private readonly IApplicationDbContext _context;

    public SellingPriceRepository(IApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SellingPriceDto>> GetDtos()
    {
        return await (from sellingPrice in _context.Set<SellingPrice>()
                      let isUsedInProductPackingUnit = _context.Set<ProductPackingUnitSellingPrice>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      let isUsedInProductSellingPriceDiscount = _context.Set<ProductSellingPriceDiscount>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      let isUsedInVariantPackingUnit = _context.Set<VariantPackingUnitSellingPrice>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      let isUsedInVariantSellingPriceDiscount = _context.Set<VariantSellingPriceDiscount>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      select new SellingPriceDto
                      {
                          Id = sellingPrice.Id,
                          Name = sellingPrice.Name,
                          NameSecondLanguage = sellingPrice.NameSecondLanguage,
                          CreatedAt = sellingPrice.CreatedAt,
                          ModifiedAt = sellingPrice.ModifiedAt,
                          IsDeletable = !(isUsedInProductPackingUnit || isUsedInProductSellingPriceDiscount || isUsedInVariantPackingUnit || isUsedInVariantSellingPriceDiscount)
                      }).ToListAsync();
    }
}
