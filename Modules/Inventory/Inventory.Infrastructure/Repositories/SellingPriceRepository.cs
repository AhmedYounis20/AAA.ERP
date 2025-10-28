using Inventory.Domain.Models.DTOS;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Repositories;

public class SellingPriceRepository : BaseSettingRepository<SellingPrice>, ISellingPriceRepository
{
    IInventoryDbContext _context;
    public SellingPriceRepository(IInventoryDbContext context) : base(context)
    => _context = context;


    public async Task<IEnumerable<SellingPriceDto>> GetDtos()
    {
        var query = await (from sellingPrice in _context.Set<SellingPrice>()
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
                           }

        ).ToListAsync();


        return query;
    }
}
