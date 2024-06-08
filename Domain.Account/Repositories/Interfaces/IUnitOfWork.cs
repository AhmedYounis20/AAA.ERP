using Domain.Account.Repositories.Interfaces.SubLeadgers;

namespace Domain.Account.Repositories.Interfaces;

public interface IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get; set; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; set; }
    public ICurrencyRepository CurrencyRepository { get; set; }
    public IGLSettingRepository GlSettingRepository { get; set; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; set; }
    public ICashInBoxRepository CashInBoxRepository { get; set; }

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}