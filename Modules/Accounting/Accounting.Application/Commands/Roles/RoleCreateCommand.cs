using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Account.Roles;

namespace Accounting.Application.Commands.Roles;

public class RoleCreateCommand : BaseSettingCreateCommand<Role>
{
    public int Commission { get; set; }
}
