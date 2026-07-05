using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Application.Commands.Roles;

public class RoleCreateCommand : BaseSettingCreateCommand<Role>
{
    public int Commission { get; set; }
}
