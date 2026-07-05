using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Accounting.Infrastructure.Repositories;

public class CollectionBookRepository : BaseSettingRepository<CollectionBook>, ICollectionBookRepository
{
    public CollectionBookRepository(IApplicationDbContext context) : base(context) { }
}
