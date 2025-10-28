using Account.Domain.Models.Entities.Roles;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.Roles;

public class RoleUpdateCommand : BaseSettingUpdateCommand<Role>
{
    public int Commission { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}