using FluentAssertions;
using System.Text.RegularExpressions;

namespace ERP.Architecture.Tests;

/// <summary>
/// Guards against reintroducing hardcoded chart-of-account GUIDs in application code.
/// </summary>
public class SubLedgerArchitectureTests
{
    private static readonly string[] SourceRoots =
    [
        "Modules/Accounting/Accounting.Infrastructure",
        "Modules/Accounting/Accounting.Application",
        "Modules/Inventory/Inventory.Infrastructure"
    ];

    private static readonly Regex LegacyCoaConstantPattern = new(
        @"Sd\.(BankChartAccountId|CustomerChartOfAccountId|SupplierChartOfAccountId|CashInBoxChartOfAccountId|BranchChartOfAccountId|StockUnderTransferBranch|AccumulatedDepreciationId|ExpensesDepreciationId|FixedAssetsIds|FixedAssetsDepreciationDetails)",
        RegexOptions.Compiled);

    [Fact]
    public void Application_code_must_not_reference_legacy_Sd_COA_constants()
    {
        var repoRoot = FindRepositoryRoot();
        var violations = new List<string>();

        foreach (var relativeRoot in SourceRoots)
        {
            var absoluteRoot = Path.Combine(repoRoot, relativeRoot);
            if (!Directory.Exists(absoluteRoot))
                continue;

            foreach (var file in Directory.EnumerateFiles(absoluteRoot, "*.cs", SearchOption.AllDirectories))
            {
                var content = File.ReadAllText(file);
                if (LegacyCoaConstantPattern.IsMatch(content))
                    violations.Add(Path.GetRelativePath(repoRoot, file));
            }
        }

        violations.Should().BeEmpty(
            "sub-ledger COA parents must be resolved via ISubLedgerCoaParentResolver, not Sd.* GUID constants. Violations:{0}{1}",
            Environment.NewLine,
            string.Join(Environment.NewLine, violations));
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "AAA.ERP.sln")))
                return current.FullName;

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }
}
