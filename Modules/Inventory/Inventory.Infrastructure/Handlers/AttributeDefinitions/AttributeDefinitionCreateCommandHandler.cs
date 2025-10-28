using Inventory.Domain.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;


namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionCreateCommandHandler(IAttributeDefinitionService service) : ICommandHandler<AttributeDefinitionCreateCommand, ApiResponse<AttributeDefinition>>
{
    public async Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
