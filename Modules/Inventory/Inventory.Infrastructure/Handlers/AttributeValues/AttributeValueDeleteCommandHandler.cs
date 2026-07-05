using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeValues;

public class AttributeValueDeleteCommandHandler(IAttributeValueService service)
    : ICommandHandler<AttributeValueDeleteCommand, ApiResponse<AttributeValue>>
{
    public Task<ApiResponse<AttributeValue>> Handle(AttributeValueDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
