using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeValues;

public class AttributeValueCreateCommandHandler(IAttributeValueService service)
    : ICommandHandler<AttributeValueCreateCommand, ApiResponse<AttributeValue>>
{
    public Task<ApiResponse<AttributeValue>> Handle(AttributeValueCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
