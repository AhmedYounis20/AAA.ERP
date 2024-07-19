using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.Roles;
using Shared.Responses;

namespace Domain.Account.Commands.Roles;

public class RoleUpdateCommand : BaseSettingUpdateCommand<Role>
{
    public int Commission { get; set; }
    
}