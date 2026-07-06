using System.Numerics;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class ChartOfAccountRepository : BaseTreeSettingRepository<ChartOfAccount>, IChartOfAccountRepository
{
    public ChartOfAccountRepository(IApplicationDbContext context) : base(context) { }

    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
    {
        ChartOfAccount? parent = await _dbSet.FirstOrDefaultAsync(e => e.Id == parentId);
        if (parent == null)
        {
            var siblingCodes = await _dbSet
                .Where(e => e.ParentId == null)
                .Select(e => e.Code)
                .ToListAsync();

            BigInteger code = 1;
            if (siblingCodes.Any())
            {
                code = siblingCodes
                    .Select(e => BigInteger.Parse(e ?? "0"))
                    .Max() + 1;
            }

            return code.ToString();
        }

        int parentLevel = await GetLevelAsync(parent);
        var siblingsCodes = await _dbSet
            .Where(e => e.ParentId == parent.Id)
            .Select(e => e.Code ?? "")
            .ToListAsync();

        BigInteger currentCode = 1;
        if (siblingsCodes.Any())
        {
            currentCode = siblingsCodes
                .Select(e => BigInteger.Parse(e.Remove(0, parent.Code?.Length ?? 0)))
                .Max() + 1;
        }

        string codeString = currentCode.ToString().PadLeft(parentLevel + 2, '0');
        return parent.Code + codeString;
    }

    private async Task<int> GetLevelAsync(ChartOfAccount chartOfAccount)
    {
        if (chartOfAccount.ParentId == null)
            return 0;

        ChartOfAccount? parent = await _dbSet.FirstOrDefaultAsync(e => e.Id == chartOfAccount.ParentId);
        return parent == null ? 0 : 1 + await GetLevelAsync(parent);
    }

    public async Task<ChartOfAccount?> GetChartOfAccountByCode(string code)
        => await _dbSet.Where(e => e.Code == code).FirstOrDefaultAsync();
}
