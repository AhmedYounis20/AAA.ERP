using Inventory.Domain.Commands.AttributeValues;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeValueCreateCommandHandler(IAttributeValueService service) : ICommandHandler<AttributeValueCreateCommand, ApiResponse<AttributeValue>>
{
    public async Task<ApiResponse<AttributeValue>> Handle(AttributeValueCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
