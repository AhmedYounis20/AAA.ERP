using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Infrastructure.Repositories;

public class CollectionBookRepository : BaseSettingRepository<CollectionBook>, ICollectionBookRepository
{
    public CollectionBookRepository(IAccountDbContext context) : base(context) { }
}
