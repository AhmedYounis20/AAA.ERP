using Accounting.Application.Commands.CollectionBooks;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.CollectionBooks;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for collection book.
/// </summary>
public interface ICollectionBookService : IBaseSettingService<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>;
