using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionUpdateCommandHandler(IAttributeDefinitionService service)
    : ICommandHandler<AttributeDefinitionUpdateCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
