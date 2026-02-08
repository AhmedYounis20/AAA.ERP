using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.InventoryThresholds;

namespace ERP.Infrastracture.Services.Inventory;

public static class InventoryThresholdValidationHelper
{
    public static (bool isValid, List<string> errors) ValidateThresholds(
        List<InventoryThresholdDto>? thresholds,
        InventoryThresholdScope? scope,
        Guid? branchId)
    {
        var errors = new List<string>();
        if (thresholds == null || !thresholds.Any())
            return (true, errors);

        // Scope and branch are at product/variant level: BranchId required when Scope = Branch
        if (scope == InventoryThresholdScope.Branch && !branchId.HasValue)
            errors.Add("INVENTORY_THRESHOLD_BRANCH_REQUIRED_WHEN_SCOPE_BRANCH");

        for (var i = 0; i < thresholds.Count; i++)
        {
            var t = thresholds[i];
            if (t.MinQuantity >= t.MaxQuantity)
                errors.Add("INVENTORY_THRESHOLD_MIN_MUST_BE_LESS_THAN_MAX");
        }

        // All thresholds share the same scope and branch; check no overlapping ranges
        var list = thresholds
            .Where(t => t.MinQuantity < t.MaxQuantity)
            .OrderBy(x => x.MinQuantity)
            .ToList();
        for (var i = 0; i < list.Count - 1; i++)
        {
            var a = list[i];
            var b = list[i + 1];
            if (a.MaxQuantity > b.MinQuantity)
                errors.Add("INVENTORY_THRESHOLD_RANGES_MUST_NOT_OVERLAP");
        }

        return (errors.Count == 0, errors.Distinct().ToList());
    }
}
