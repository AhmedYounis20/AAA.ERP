using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public sealed class CollectionBookUpdateCommandHandler(ICollectionBookRepository repository)
    : BaseSettingUpdateHandler<CollectionBook, CollectionBookUpdateCommand>(repository);
