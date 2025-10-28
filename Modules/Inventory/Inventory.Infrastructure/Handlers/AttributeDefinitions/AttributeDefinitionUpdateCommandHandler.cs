using Inventory.Domain.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionUpdateCommandHandler(IAttributeDefinitionService service) : ICommandHandler<AttributeDefinitionUpdateCommand, ApiResponse<AttributeDefinition>>
{
    public async Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}