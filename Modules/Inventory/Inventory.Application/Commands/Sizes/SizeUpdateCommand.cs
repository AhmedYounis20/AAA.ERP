using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Commands.Sizes;

public class SizeUpdateCommand : BaseSettingUpdateCommand<Size>
{
    public string Code { get; set; } = string.Empty;
}
