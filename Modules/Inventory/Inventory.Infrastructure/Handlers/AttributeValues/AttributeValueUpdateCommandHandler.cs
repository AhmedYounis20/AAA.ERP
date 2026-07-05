using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeValues;

public class AttributeValueUpdateCommandHandler(IAttributeValueService service)
    : ICommandHandler<AttributeValueUpdateCommand, ApiResponse<AttributeValue>>
{
    public Task<ApiResponse<AttributeValue>> Handle(AttributeValueUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
