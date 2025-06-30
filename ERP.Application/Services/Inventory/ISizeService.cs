using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.Application.Services.Inventory;

public interface ISizeService : IBaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand> { }
