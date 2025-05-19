using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Application.Services.Inventory;

public interface ISellingPriceService : IBaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand> { }