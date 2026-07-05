using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Collection book read queries. Transitional: delegates to <see cref="ICollectionBookService"/> until write methods are removed.
/// </summary>
public class CollectionBookQueries(ICollectionBookService collectionBookService)
    : BaseSettingQueries<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>(collectionBookService),
      ICollectionBookQueries;
