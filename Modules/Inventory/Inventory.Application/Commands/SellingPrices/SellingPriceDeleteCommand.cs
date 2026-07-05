using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.SellingPrices;
using Shared;

namespace Inventory.Application.Commands.SellingPrices;

/// <summary>Deletes a selling price record.</summary>
public record SellingPriceDeleteCommand(Guid Id) : ICommand<ApiResponse<SellingPrice>>;
