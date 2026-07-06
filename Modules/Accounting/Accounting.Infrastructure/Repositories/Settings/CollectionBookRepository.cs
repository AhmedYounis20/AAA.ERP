using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Accounting.Infrastructure.Repositories;

public class CollectionBookRepository : BaseSettingRepository<CollectionBook>, ICollectionBookRepository
{
    public CollectionBookRepository(IApplicationDbContext context) : base(context) { }
}
