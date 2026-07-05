using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Handlers.AttributeDefinitions;

public class AttributeDefinitionDeleteCommandHandler(IAttributeDefinitionService service)
    : ICommandHandler<AttributeDefinitionDeleteCommand, ApiResponse<AttributeDefinition>>
{
    public Task<ApiResponse<AttributeDefinition>> Handle(AttributeDefinitionDeleteCommand request, CancellationToken cancellationToken)
        => service.Delete(request.Id);
}
