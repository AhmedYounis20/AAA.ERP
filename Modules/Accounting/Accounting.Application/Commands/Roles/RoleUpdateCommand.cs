using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Application.Commands.Roles;

public class RoleUpdateCommand : BaseSettingUpdateCommand<Role>
{
    public int Commission { get; set; }
}
