using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Repositories;

public interface ICollectionBookRepository : IBaseSettingRepository<CollectionBook>;
