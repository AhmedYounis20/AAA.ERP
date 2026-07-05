using ERP.Application.Repositories;

namespace ERP.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationDbContext _context;
    private IDbContextTransaction? _sqlTransaction;

    public UnitOfWork(IApplicationDbContext context)
    {
        _context = context;
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

    public DbSet<T> Set<T>() where T : BaseEntity
    {
        return _context.Set<T>();
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
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
}
