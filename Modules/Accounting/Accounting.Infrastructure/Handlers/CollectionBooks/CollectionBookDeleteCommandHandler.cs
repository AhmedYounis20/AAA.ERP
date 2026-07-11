using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Accounting.Infrastructure.Handlers.CollectionBooks;

public sealed class CollectionBookDeleteCommandHandler(ICollectionBookRepository repository)
    : BaseSettingDeleteHandler<CollectionBook, CollectionBookDeleteCommand>(repository);
