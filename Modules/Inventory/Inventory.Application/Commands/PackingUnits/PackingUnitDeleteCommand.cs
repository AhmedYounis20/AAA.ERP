using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.PackingUnits;
using Shared;

namespace Inventory.Application.Commands.PackingUnits;

/// <summary>Deletes a packing unit record.</summary>
public record PackingUnitDeleteCommand(Guid Id) : ICommand<ApiResponse<PackingUnit>>;
