using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.Roles;

public class Role : BaseSettingEntity
{
    public int Commission { get; set; }
}