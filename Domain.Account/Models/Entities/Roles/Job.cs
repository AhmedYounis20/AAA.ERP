using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Roles;

public class Role : BaseSettingEntity
{
    public int Commission { get; set; }
}