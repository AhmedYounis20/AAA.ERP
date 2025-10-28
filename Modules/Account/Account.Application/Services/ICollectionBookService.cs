using Account.Domain.Commands.CollectionBooks;
using Account.Domain.Models.Entities.CollectionBooks;
using Shared.Application.BaseServices;

namespace Account.Application.Services;

public interface ICollectionBookService : IBaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand> { }