using Inventory.Application.Dtos;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Validation;

public static class ExpiryLevelValidationHelper
{
    public static (bool isValid, List<string> errors) ValidateExpiryLevels(List<ExpiryLevelDto>? expiryLevels)
    {
        var errors = new List<string>();
        if (expiryLevels == null || !expiryLevels.Any())
            return (true, errors);

        for (var i = 0; i < expiryLevels.Count; i++)
        {
            var el = expiryLevels[i];
            if (el.DaysFrom >= el.DaysTo)
                errors.Add(ErrorMessages.EXPIRY_LEVEL_DAYS_FROM_MUST_BE_LESS_THAN_DAYS_TO);
        }

        var list = expiryLevels
            .Where(el => el.DaysFrom < el.DaysTo)
            .OrderBy(x => x.DaysFrom)
            .ToList();
        for (var i = 0; i < list.Count - 1; i++)
        {
            var a = list[i];
            var b = list[i + 1];
            if (a.DaysTo > b.DaysFrom)
                errors.Add(ErrorMessages.EXPIRY_LEVEL_RANGES_MUST_NOT_OVERLAP);
        }

        return (errors.Count == 0, errors.Distinct().ToList());
    }
}
