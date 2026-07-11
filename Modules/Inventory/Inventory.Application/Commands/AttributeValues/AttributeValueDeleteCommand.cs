using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Attributes;
using Shared;

namespace Inventory.Application.Commands.AttributeValues;

/// <summary>Deletes an attribute value record.</summary>
public record AttributeValueDeleteCommand(Guid Id) : ICommand<ApiResponse<AttributeValue>>, IEntityDeleteCommand;
