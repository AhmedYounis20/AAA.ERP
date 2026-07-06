using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public sealed class CollectionBookCreateCommandHandler(ICollectionBookRepository repository)
    : BaseSettingCreateHandler<CollectionBook, CollectionBookCreateCommand>(repository);
