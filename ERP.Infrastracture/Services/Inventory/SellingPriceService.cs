using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Infrastracture.Services.Inventory;
public class SellingPriceService :
    BaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>, ISellingPriceService
{
    public SellingPriceService(ISellingPriceRepository repository) : base(repository)
    { }
}