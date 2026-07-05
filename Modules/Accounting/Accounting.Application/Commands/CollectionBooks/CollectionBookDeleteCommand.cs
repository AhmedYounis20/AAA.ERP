using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.CollectionBooks;

public record CollectionBookDeleteCommand(Guid Id) : ICommand<ApiResponse<CollectionBook>>;
