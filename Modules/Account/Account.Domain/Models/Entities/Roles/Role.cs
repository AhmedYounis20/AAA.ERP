using Shared.Domain.BaseEntities;

namespace Account.Domain.Models.Entities.Roles;

public class Role : BaseSettingEntity
{
    public int Commission { get; set; }
}
