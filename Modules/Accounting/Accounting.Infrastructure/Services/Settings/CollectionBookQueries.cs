using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Collection book read queries.
/// </summary>
public class CollectionBookQueries(ICollectionBookRepository repository)
    : BaseSettingRepositoryQueries<CollectionBook>(repository),
      ICollectionBookQueries;
