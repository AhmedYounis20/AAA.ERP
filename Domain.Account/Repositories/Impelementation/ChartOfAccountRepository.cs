using System.Numerics;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Repositories.Interfaces;
using Shared.BaseRepositories.Impelementation;

namespace Domain.Account.Repositories.Impelementation;

public class ChartOfAccountRepository : BaseTreeSettingRepository<ChartOfAccount>, IChartOfAccountRepository
{

    public ChartOfAccountRepository(ApplicationDbContext context) : base(context) { }


    public async Task<string> GenerateNewCodeForChild(Guid? parentId)
    {
        ChartOfAccount? parent = await _dbSet.FirstOrDefaultAsync(e => e.Id == parentId);
        if (parent == null)
        {
            // If parent not found, generate code based on siblings
            var siblingCodes = await _dbSet
                .Where(e => e.ParentId == parentId)
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
        else
        {
            // If parent found, generate code based on parent
            int parentLevel = await GetLevelAsync(parent);
            var siblingsCodes = await _dbSet
                .Where(e => e.ParentId == parent.Id)
                .Select(e => e.Code ?? "")
                .ToListAsync();

            BigInteger currentCode = 1;
            if (siblingsCodes.Any())
            {
                currentCode = siblingsCodes
                    .Select(e => BigInteger.Parse(e.Remove(0, parent.Code?.Length ??0)))
                    .Max() + 1;
            }

            string codeString = currentCode.ToString().PadLeft(parentLevel + 2, '0');
            return parent.Code + codeString;
        }
    }

    private async Task<int> GetLevelAsync(ChartOfAccount chartOfAccount)
    {
        if (chartOfAccount.ParentId == null)
        {
            return 0;
        }
        else
        {
            ChartOfAccount? parent = await _dbSet.FirstOrDefaultAsync(e => e.Id == chartOfAccount.ParentId);
            if (parent == null)
            {
                return 0;
            }

            return 1 + await GetLevelAsync(parent);
        }
    }
    public async Task<ChartOfAccount?> GetChartOfAccountByCode(string code)
    => await _dbSet.Where(e=>e.Code ==  code).FirstOrDefaultAsync();
}