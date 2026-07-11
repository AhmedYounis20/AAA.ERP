using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Domain.Models.Entities.Roles;

public class Role : BaseSettingEntity
{
    public int Commission { get; set; }
}