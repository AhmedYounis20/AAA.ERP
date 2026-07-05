using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Variants;
using Shared;

namespace Inventory.Application.Commands.Variants;

/// <summary>Deletes a variant record.</summary>
public record VariantDeleteCommand(Guid Id) : ICommand<ApiResponse<Variant>>;
