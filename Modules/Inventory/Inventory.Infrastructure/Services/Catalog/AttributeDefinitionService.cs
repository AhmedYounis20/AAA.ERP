using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute definition read-side helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public class AttributeDefinitionService(IAttributeDefinitionRepository attributeDefinitionRepository)
    : IAttributeDefinitionService
{
    public async Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
    {
        var entity = await attributeDefinitionRepository.GetWithPredefinedValuesAsync(id);
        if (entity is null)
            return null;

        return new AttributeDefinitionDto
        {
            Id = entity.Id,
            IsActive = entity.IsActive,
            Name = entity.Name,
            NameSecondLanguage = entity.NameSecondLanguage,
            PredefinedValues = entity.PredefinedValues.Select(e => e.Adapt<AttributeValueDto>()).ToList()
        };
    }

    public async Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
    {
        var entities = await attributeDefinitionRepository.GetActiveAttributeDefinitionsAsync();
        return entities.Select(entity => new AttributeDefinitionDto
        {
            Id = entity.Id,
            IsActive = entity.IsActive,
            Name = entity.Name,
            NameSecondLanguage = entity.NameSecondLanguage,
            PredefinedValues = entity.PredefinedValues.Select(e => e.Adapt<AttributeValueDto>()).ToList()
        }).ToList();
    }
}
