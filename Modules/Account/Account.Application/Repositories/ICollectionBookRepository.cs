using Account.Domain.Models.Entities.CollectionBooks;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface ICollectionBookRepository : IBaseSettingRepository<CollectionBook> { }