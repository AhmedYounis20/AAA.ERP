using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Products;
using Shared;

namespace Inventory.Application.Commands.Products;

/// <summary>Deletes a product record.</summary>
public record ProductDeleteCommand(Guid Id) : ICommand<ApiResponse<Product>>;
