using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Colors;
using Shared;

namespace Inventory.Application.Commands.Colors;

/// <summary>Deletes a color record.</summary>
public record ColorDeleteCommand(Guid Id) : ICommand<ApiResponse<Color>>;
