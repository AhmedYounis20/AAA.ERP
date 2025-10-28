using Account.Domain.Models.Entities.Roles;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Domain.Commands.Roles;

public class RoleCreateCommand : BaseSettingCreateCommand<Role>
{
    public int Commission { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}