using Account.Application.Repositories;
using Account.Domain.Commands.CollectionBooks;
using Account.Domain.Models.Entities.CollectionBooks;

namespace Account.Infrastructure.Services;
public class CollectionBookService : 
    BaseSettingService<CollectionBook,CollectionBookCreateCommand,CollectionBookUpdateCommand>, ICollectionBookService
{
    public CollectionBookService(ICollectionBookRepository repository) : base(repository)
    { }
}
