using Account.Application.Repositories.SubLeadgers;
using Account.Domain.Models.Entities.Entries;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Repositories;
using Shared.Domain.BaseEntities;

namespace Account.Application.Repositories;

public interface IUnitOfWork
{
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
    public DbSet<T> Set<T>() where T : BaseEntity;
    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
    public Task<int> SaveChanges();
}