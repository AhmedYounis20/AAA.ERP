using Inventory.Application.Repositories;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.SellingPrices;
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
                      let isUsedInItemPackngUnit = _context.Set<ItemPackingUnitSellingPrice>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      let isUsedInItemSellingPriceDiscount = _context.Set<ItemSellingPriceDiscount>().Any(e => e.SellingPriceId == sellingPrice.Id)
                      select new SellingPriceDto
                      {
                          Id = sellingPrice.Id,
                          Name = sellingPrice.Name,
                          NameSecondLanguage = sellingPrice.NameSecondLanguage,
                          CreatedAt = sellingPrice.CreatedAt,
                          ModifiedAt = sellingPrice.ModifiedAt,
                          IsDeletable = !(isUsedInItemPackngUnit || isUsedInItemSellingPriceDiscount)
                      }).ToListAsync();
    }
}
