using Domain.Account.Models.BaseEntities;

namespace Domain.Account.Models.BaseEntities;

public class BaseSettingEntity : BaseEntity
{
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}