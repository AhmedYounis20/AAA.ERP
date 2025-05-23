using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.Items;
using Shared.Responses;

namespace ERP.Application.Services.Inventory;

public interface IItemService : IBaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand> {

    Task<ApiResponse<string>> GeNextCode(Guid? parentId = null);
}