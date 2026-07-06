using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

/// <summary>
/// Shared duplicate-name validation lifted from <c>BaseSettingService</c>.
/// </summary>
internal static class SettingDuplicateNameValidator
{
    public static List<string> CollectDuplicateNameErrors(
        BaseSettingEntity existedEntity,
        string? name,
        string? nameSecondLanguage)
    {
        var errors = new List<string>();

        if (existedEntity.Name.Trim().ToUpper() == name?.Trim().ToUpper())
            errors.Add("WithSameNameIsExisted");

        if (existedEntity.NameSecondLanguage.Trim().ToUpper() == nameSecondLanguage?.Trim().ToUpper())
            errors.Add("WithSameNameSecondLanguageIsExisted");

        return errors;
    }
}
