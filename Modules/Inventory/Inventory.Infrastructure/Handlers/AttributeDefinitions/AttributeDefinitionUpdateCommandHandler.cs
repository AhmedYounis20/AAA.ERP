using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionUpdateCommandHandler(IAttributeDefinitionService service)
    : ICommandHandler<AttributeDefinitionUpdateCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
