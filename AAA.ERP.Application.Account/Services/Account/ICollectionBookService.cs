using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace ERP.Application.Services.Account;

public interface ICollectionBookService : IBaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand> { }