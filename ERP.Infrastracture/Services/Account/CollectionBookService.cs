using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace AAA.ERP.Services.Impelementation;
public class CollectionBookService : 
    BaseSettingService<CollectionBook,CollectionBookCreateCommand,CollectionBookUpdateCommand>, ICollectionBookService
{
    public CollectionBookService(ICollectionBookRepository repository) : base(repository)
    { }
}
