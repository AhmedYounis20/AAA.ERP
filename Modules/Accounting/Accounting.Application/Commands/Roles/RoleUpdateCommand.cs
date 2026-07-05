using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Application.Commands.Roles;

public class RoleUpdateCommand : BaseSettingUpdateCommand<Role>
{
    public int Commission { get; set; }
}
