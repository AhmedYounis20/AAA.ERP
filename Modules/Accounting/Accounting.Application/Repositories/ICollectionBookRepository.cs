using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for collection book persistence.
/// </summary>
public interface ICollectionBookRepository : IBaseSettingRepository<CollectionBook>;
