using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.AccountGuide;
using Domain.Account.Models.Entities.CollectionBooks;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;

namespace AAA.ERP.Services.Impelementation;

using AAA.ERP.Services.Interfaces;

public class CollectionBookService : 
    BaseSettingService<CollectionBook,CollectionBookCreateCommand,CollectionBookUpdateCommand>, ICollectionBookService
{
    public CollectionBookService(ICollectionBookRepository repository) : base(repository)
    { }
}
