using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.Roles;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.Roles;

public class RoleUpdateCommand : BaseSettingUpdateCommand<Role>
{
    public int Commission { get; set; }

}