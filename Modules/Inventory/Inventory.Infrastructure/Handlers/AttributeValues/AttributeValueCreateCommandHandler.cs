using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeValues;

public class AttributeValueCreateCommandHandler(IAttributeValueService service)
    : ICommandHandler<AttributeValueCreateCommand, ApiResponse<AttributeValue>>
{
    public Task<ApiResponse<AttributeValue>> Handle(AttributeValueCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
