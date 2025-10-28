using Inventory.Domain.Commands.AttributeValues;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeValueUpdateCommandHandler(IAttributeValueService service) : ICommandHandler<AttributeValueUpdateCommand, ApiResponse<AttributeValue>>
{
    public async Task<ApiResponse<AttributeValue>> Handle(AttributeValueUpdateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}