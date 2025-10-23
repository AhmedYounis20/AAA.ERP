using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Handlers.Inventory.AttributeDefinitions;

public class AttributeValueCreateCommandHandler(IAttributeValueService service) : ICommandHandler<AttributeValueCreateCommand, ApiResponse<AttributeValue>>
{
    public async Task<ApiResponse<AttributeValue>> Handle(AttributeValueCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
