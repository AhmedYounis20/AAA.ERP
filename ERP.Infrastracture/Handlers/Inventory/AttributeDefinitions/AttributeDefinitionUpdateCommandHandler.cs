using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Handlers.Inventory.AttributeDefinitions;

public class AttributeDefinitionUpdateCommandHandler(IAttributeDefinitionService service) : ICommandHandler<AttributeDefinitionUpdateCommand, ApiResponse<AttributeDefinition>>
{
    public async Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}