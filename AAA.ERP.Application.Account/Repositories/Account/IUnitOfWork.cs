using ERP.Application.Repositories.SubLeadgers;

namespace ERP.Application.Repositories;

public interface IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get; set; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; set; }
    public ICurrencyRepository CurrencyRepository { get; set; }
    public IGLSettingRepository GlSettingRepository { get; set; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; set; }
    public ICashInBoxRepository CashInBoxRepository { get; set; }
    public IAttachmentRepository AttachmentRepository { get; set; }
    public IBranchRepository BranchRepository { get; set; }

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}