using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.CostCenters;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Repositories;

public class ChartOfAccountRepository : BaseTreeSettingRepository<ChartOfAccount>, IChartOfAccountRepository
{
    private readonly IApplicationDbContext _context;

    public ChartOfAccountRepository(IApplicationDbContext context) : base(context)
        => _context = context;

    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
    {
        ChartOfAccount? parent = parentId is null
            ? null
            : await _dbSet.FirstOrDefaultAsync(e => e.Id == parentId);

        if (parent is null)
        {
            var siblingCodes = await _dbSet
                .Where(e => e.ParentId == null)
                .Select(e => e.Code)
                .ToListAsync();

            var code = BigInteger.One;
            if (siblingCodes.Count > 0)
            {
                var suffixes = ParseSiblingSuffixes(siblingCodes, 0);
                if (suffixes.Count == 0)
                    throw new InvalidOperationException(ErrorMessages.InvalidChartOfAccountCode);

                code = suffixes.Max() + 1;
            }

            return code.ToString();
        }

        var parentLevel = await GetLevelAsync(parent);
        var siblingsCodes = await _dbSet
            .Where(e => e.ParentId == parent.Id)
            .Select(e => e.Code ?? string.Empty)
            .ToListAsync();

        var parentCodeLength = parent.Code?.Length ?? 0;
        var currentCode = BigInteger.One;
        if (siblingsCodes.Count > 0)
        {
            var suffixes = ParseSiblingSuffixes(siblingsCodes, parentCodeLength);
            if (suffixes.Count == 0)
                throw new InvalidOperationException(ErrorMessages.InvalidChartOfAccountCode);

            currentCode = suffixes.Max() + 1;
        }

        var codeString = currentCode.ToString().PadLeft(parentLevel + 2, '0');
        return parent.Code + codeString;
    }

    public async Task<ChartOfAccount?> GetChartOfAccountByCode(string code)
    {
        var normalized = code.Trim();
        return await _dbSet.FirstOrDefaultAsync(e => e.Code != null && e.Code.Trim() == normalized);
    }

    public Task<bool> IsUsedInEntries(Guid id)
        => _context.Set<FinancialTransaction>().AnyAsync(e => e.ChartOfAccountId == id);

    public Task<bool> IsUsedInCostCenters(Guid id)
        => _context.Set<CostCenterChartOfAccount>().AnyAsync(e => e.ChartOfAccountId == id);

    public async Task<bool> IsUsedInSubLedgers(Guid id)
    {
        if (await _context.Set<Branch>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;
        if (await _context.Set<Customer>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;
        if (await _context.Set<Supplier>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;
        if (await _context.Set<CashInBox>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;
        if (await _context.Set<Bank>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;
        if (await _context.Set<Tax>().AnyAsync(e => e.ChartOfAccountId == id))
            return true;

        return await _context.Set<FixedAsset>().AnyAsync(e =>
            e.ChartOfAccountId == id ||
            e.AccumlatedAccountId == id ||
            e.ExpensesAccountId == id);
    }

    public async Task<bool> IsInUse(Guid id)
    {
        if (await IsUsedInEntries(id))
            return true;
        if (await IsUsedInCostCenters(id))
            return true;

        return await IsUsedInSubLedgers(id);
    }

    private async Task<int> GetLevelAsync(ChartOfAccount chartOfAccount)
    {
        if (chartOfAccount.ParentId == null)
            return 0;

        var parent = await _dbSet.FirstOrDefaultAsync(e => e.Id == chartOfAccount.ParentId);
        return parent is null ? 0 : 1 + await GetLevelAsync(parent);
    }

    private static List<BigInteger> ParseSiblingSuffixes(IEnumerable<string> siblingCodes, int parentCodeLength)
    {
        var suffixes = new List<BigInteger>();

        foreach (var siblingCode in siblingCodes)
        {
            if (string.IsNullOrWhiteSpace(siblingCode))
                continue;

            var suffixPart = parentCodeLength == 0
                ? siblingCode.Trim()
                : siblingCode.Length <= parentCodeLength
                    ? null
                    : siblingCode[parentCodeLength..].Trim();

            if (suffixPart is null || !BigInteger.TryParse(suffixPart, out var suffix))
                continue;

            suffixes.Add(suffix);
        }

        return suffixes;
    }
}
