using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Sizes;
using Shared;

namespace Inventory.Application.Commands.Sizes;

/// <summary>Deletes a size record.</summary>
public record SizeDeleteCommand(Guid Id) : ICommand<ApiResponse<Size>>, IEntityDeleteCommand;
