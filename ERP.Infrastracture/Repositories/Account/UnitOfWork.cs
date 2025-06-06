using ERP.Application.Repositories.Account;
using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Application.Repositories.Inventory;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.Inventory;

namespace ERP.Infrastracture.Repositories.Account;

public class UnitOfWork : IUnitOfWork
{
    public IAccountGuideRepository AccountGuideRepository { get; set; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; set; }
    public ICurrencyRepository CurrencyRepository { get; set; }
    public IGLSettingRepository GlSettingRepository { get; set; }
    public IEntryRepository EntryRepository { get; set; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; set; }
    public IAttachmentRepository AttachmentRepository { get; set; }
    public ICashInBoxRepository CashInBoxRepository { get; set; }
    public IBranchRepository BranchRepository { get; set; }
    public IPackingUnitRepository PackingUnitRepository { get; set; }
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
        AttachmentRepository = new AttachmentRepository(context);
        BranchRepository = new BranchRepository(context);
        EntryRepository = new EntryRepository(context);
        PackingUnitRepository = new PackingUnitRepository(context);
        _context = context;
    }

    public async Task BeginTransactionAsync()
        => _sqlTransaction = _sqlTransaction ?? await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        if (_sqlTransaction is not null)
            await _sqlTransaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_sqlTransaction is not null)
            await _sqlTransaction.RollbackAsync();
    }
}