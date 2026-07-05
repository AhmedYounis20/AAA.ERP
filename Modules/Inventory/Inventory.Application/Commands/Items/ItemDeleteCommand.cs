using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Items;
using Shared;

namespace Inventory.Application.Commands.Items;

/// <summary>Deletes an item record.</summary>
public record ItemDeleteCommand(Guid Id) : ICommand<ApiResponse<Item>>;
