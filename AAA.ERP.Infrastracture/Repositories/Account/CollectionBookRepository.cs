using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class CollectionBookRepository : BaseSettingRepository<CollectionBook>, ICollectionBookRepository
{
    public CollectionBookRepository(ApplicationDbContext context) : base(context) { }
}
