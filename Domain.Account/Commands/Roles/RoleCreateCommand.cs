using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Models.Entities.Roles;
using Shared.Responses;

namespace Domain.Account.Commands.Roles;

public class RoleCreateCommand : BaseSettingCreateCommand<Role>
{
    public int Commission { get; set; }
}