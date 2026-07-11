using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Attribute definition read queries.
/// </summary>
public class AttributeDefinitionQueries(IAttributeDefinitionRepository repository)
    : BaseSettingRepositoryQueries<AttributeDefinition>(repository),
      IAttributeDefinitionQueries
{
    public async Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
    {
        var entities = await repository.GetActiveAttributeDefinitionsAsync();
        return entities.Select(entity => new AttributeDefinitionDto
        {
            Id = entity.Id,
            IsActive = entity.IsActive,
            Name = entity.Name,
            NameSecondLanguage = entity.NameSecondLanguage,
            PredefinedValues = entity.PredefinedValues.Select(e => e.Adapt<AttributeValueDto>()).ToList()
        }).ToList();
    }

    public async Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
    {
        var entity = await repository.GetWithPredefinedValuesAsync(id);
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
}
