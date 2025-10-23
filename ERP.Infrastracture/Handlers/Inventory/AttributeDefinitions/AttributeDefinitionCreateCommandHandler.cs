using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Handlers.Inventory.AttributeDefinitions;

public class AttributeDefinitionCreateCommandHandler(IAttributeDefinitionService service) : ICommandHandler<AttributeDefinitionCreateCommand, ApiResponse<AttributeDefinition>>
{
    public async Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
