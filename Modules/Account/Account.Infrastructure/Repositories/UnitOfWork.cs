using Account.Application.Data;
using Account.Application.Repositories;
using Account.Application.Repositories.SubLeadgers;
using Account.Domain.Models.Entities.Entries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Application.Repositories;
using Shared.Domain.BaseEntities;

namespace Account.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IAccountDbContext _context;
    private IDbContextTransaction? _sqlTransaction;

    public IAccountGuideRepository AccountGuideRepository { get; }
    public IChartOfAccountRepository ChartOfAccountRepository { get; }
    public ICurrencyRepository CurrencyRepository { get; }
    public IGLSettingRepository GlSettingRepository { get; }
    public IFinancialPeriodRepository FinancialPeriodRepository { get; }
    public ICashInBoxRepository CashInBoxRepository { get; }
    public IAttachmentRepository AttachmentRepository { get; }
    public IBranchRepository BranchRepository { get; }
    public ISupplierRepository SupplierRepository { get; }
    public IEntryRepository EntryRepository { get; }
    public IBaseRepository<FinancialTransaction> FinancialTransactionRepository { get; }
    public IBaseRepository<EntryAttachment> EntryAttachmentRepository { get; }

    public UnitOfWork(
        IAccountDbContext context,
        IAccountGuideRepository accountGuideRepository,
        IChartOfAccountRepository chartOfAccountRepository,
        ICurrencyRepository currencyRepository,
        IGLSettingRepository glSettingRepository,
        IFinancialPeriodRepository financialPeriodRepository,
        ICashInBoxRepository cashInBoxRepository,
        IAttachmentRepository attachmentRepository,
        IBranchRepository branchRepository,
        ISupplierRepository supplierRepository,
        IEntryRepository entryRepository,
        IBaseRepository<FinancialTransaction> financialTransactionRepository,
        IBaseRepository<EntryAttachment> entryAttachmentRepository
    )
    {
        _context = context;
        AccountGuideRepository = accountGuideRepository;
        ChartOfAccountRepository = chartOfAccountRepository;
        CurrencyRepository = currencyRepository;
        GlSettingRepository = glSettingRepository;
        FinancialPeriodRepository = financialPeriodRepository;
        CashInBoxRepository = cashInBoxRepository;
        AttachmentRepository = attachmentRepository;
        BranchRepository = branchRepository;
        SupplierRepository = supplierRepository;
        EntryRepository = entryRepository;
        FinancialTransactionRepository = financialTransactionRepository;
        EntryAttachmentRepository = entryAttachmentRepository;
    }

    public async Task BeginTransactionAsync()
    {
        _sqlTransaction ??= await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_sqlTransaction is not null)
        {
            await _sqlTransaction.CommitAsync();
            await _sqlTransaction.DisposeAsync();
            _sqlTransaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_sqlTransaction is not null)
        {
            await _sqlTransaction.RollbackAsync();
            await _sqlTransaction.DisposeAsync();
            _sqlTransaction = null;
        }
    }

    public DbSet<T> Set<T>() where T : BaseEntity => _context.Set<T>() as DbSet<T> ?? throw new InvalidOperationException("DbSet not available");

    public Task<int> SaveChanges() => _context.SaveChangesAsync();
}


