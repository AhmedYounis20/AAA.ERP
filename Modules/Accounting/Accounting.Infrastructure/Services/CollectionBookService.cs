using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.CollectionBooks;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class CollectionBookService
    : BaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>, ICollectionBookService
{
    public CollectionBookService(ICollectionBookRepository repository) : base(repository) { }
}
