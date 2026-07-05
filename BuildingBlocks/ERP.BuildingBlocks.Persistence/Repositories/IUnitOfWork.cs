using Microsoft.EntityFrameworkCore;
using Shared.BaseEntities;

namespace ERP.Application.Repositories;

/// <summary>
/// Transaction coordinator for cross-aggregate writes. Repository access is via direct DI injection.
/// </summary>
public interface IUnitOfWork
{
    DbSet<T> Set<T>() where T : BaseEntity;
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<int> SaveChanges();
}
