using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Account.Roles;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.Roles;

public class RoleCreateCommand : BaseSettingCreateCommand<Role>
{
    public int Commission { get; set; }
}