using Domain.Account.Commands.AccountGuides;
using Domain.Account.Models.Entities.CollectionBooks;
using ERP.Application.Services.BaseServices;

namespace ERP.Application.Services.Account;

public interface ICollectionBookService : IBaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand> { }