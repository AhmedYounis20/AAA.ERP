using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class SellingPriceRepository : BaseSettingRepository<SellingPrice>, ISellingPriceRepository
{
    IApplicationDbContext _context;
    public SellingPriceRepository(IApplicationDbContext context) : base(context)
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