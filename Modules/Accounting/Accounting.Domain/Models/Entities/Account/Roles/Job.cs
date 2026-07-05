using Shared.BaseEntities;

namespace Accounting.Domain.Models.Entities.Roles;

public class Role : BaseSettingEntity
{
    public int Commission { get; set; }
}