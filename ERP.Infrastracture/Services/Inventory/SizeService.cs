using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.Infrastracture.Services.Inventory;
public class SizeService :
    BaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>, ISizeService
{
    public SizeService(ISizeRepository repository) : base(repository)
    { }
}