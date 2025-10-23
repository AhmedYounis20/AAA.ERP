using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Handlers.Inventory.AttributeDefinitions;

public class AttributeValueUpdateCommandHandler(IAttributeValueService service) : ICommandHandler<AttributeValueUpdateCommand, ApiResponse<AttributeValue>>
{
    public async Task<ApiResponse<AttributeValue>> Handle(AttributeValueUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}