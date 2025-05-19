using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Application.Services.Account;

namespace AAA.ERP.Services.Impelementation;
public class CollectionBookService : 
    BaseSettingService<CollectionBook,CollectionBookCreateCommand,CollectionBookUpdateCommand>, ICollectionBookService
{
    public CollectionBookService(ICollectionBookRepository repository) : base(repository)
    { }
}
