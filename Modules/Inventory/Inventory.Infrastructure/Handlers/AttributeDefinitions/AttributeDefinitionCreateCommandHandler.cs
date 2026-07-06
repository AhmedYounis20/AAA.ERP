using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public sealed class AttributeDefinitionCreateCommandHandler(IAttributeDefinitionWorkflowService workflow)
    : ICommandHandler<AttributeDefinitionCreateCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(
        AttributeDefinitionCreateCommand request,
        CancellationToken cancellationToken)
        => workflow.Create(request);
}

public sealed class AttributeDefinitionUpdateCommandHandler(IAttributeDefinitionWorkflowService workflow)
    : ICommandHandler<AttributeDefinitionUpdateCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(
        AttributeDefinitionUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}

public sealed class AttributeDefinitionDeleteCommandHandler(IAttributeDefinitionWorkflowService workflow)
    : ICommandHandler<AttributeDefinitionDeleteCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(
        AttributeDefinitionDeleteCommand request,
        CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
