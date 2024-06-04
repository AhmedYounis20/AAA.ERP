using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Repositories.Impelementation.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Account.Repositories.Impelementation;

public class UnitOfWork : IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get; set; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; set; }
    public ICurrencyRepository CurrencyRepository { get; set; }
    public IGLSettingRepository GlSettingRepository { get; set; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; set; }
    public ICashInBoxRepository CashInBoxRepository { get; set; }
    private IDbContextTransaction? _sqlTransaction;
    private ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        AccountGuideRepository = new AccountGuideRepository(context);
        ChartOfAccountRepository = new ChartOfAccountRepository(context);
        CurrencyRepository = new CurrencyRepository(context);
        CashInBoxRepository = new CashInBoxRepository(context);
        GlSettingRepository = new GLSettingRepository(context);
        FinancialPeriodRepository = new FinancialPeriodRepository(context);
        _context = context;
    }

    public async Task BeginTransactionAsync()
        => _sqlTransaction = _sqlTransaction ?? await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        if(_sqlTransaction is not null)
            await _sqlTransaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_sqlTransaction is not null)
            await _sqlTransaction.RollbackAsync();
    }
}