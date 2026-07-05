using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionCreateCommandHandler(IAttributeDefinitionService service)
    : ICommandHandler<AttributeDefinitionCreateCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
