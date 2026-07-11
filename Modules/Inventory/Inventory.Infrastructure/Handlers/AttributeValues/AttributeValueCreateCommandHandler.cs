using Inventory.Application.Commands.AttributeValues;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.AttributeValues;

public sealed class AttributeValueCreateCommandHandler(IAttributeValueRepository repository)
    : BaseSettingCreateHandler<AttributeValue, AttributeValueCreateCommand>(repository);
