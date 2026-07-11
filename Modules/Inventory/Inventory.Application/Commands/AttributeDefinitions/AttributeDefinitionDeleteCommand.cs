using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Attributes;
using Shared;

namespace Inventory.Application.Commands.AttributeDefinitions;

/// <summary>Deletes an attribute definition record.</summary>
public record AttributeDefinitionDeleteCommand(Guid Id) : ICommand<ApiResponse<AttributeDefinition>>;
