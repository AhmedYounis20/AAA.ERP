using Accounting.Application.Commands.CollectionBooks;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.CollectionBooks;

namespace Accounting.Application.Services;

public interface ICollectionBookService : IBaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>;
