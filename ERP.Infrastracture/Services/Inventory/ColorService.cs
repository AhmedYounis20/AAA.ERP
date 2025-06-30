using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace ERP.Infrastracture.Services.Inventory;
public class ColorService :
    BaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>, IColorService
{
    public ColorService(IColorRepository repository) : base(repository)
    { }
}