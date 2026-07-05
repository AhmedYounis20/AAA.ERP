using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Commands.Sizes;

public class SizeUpdateCommand : BaseSettingUpdateCommand<Size>
{
    public string Code { get; set; } = string.Empty;
}
