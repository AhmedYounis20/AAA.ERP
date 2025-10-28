using Inventory.Domain.Commands.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;
using Mapster;


namespace Inventory.Infrastructure.Services;

public class AttributeDefinitionService : BaseSettingService<AttributeDefinition, AttributeDefinitionCreateCommand, AttributeDefinitionUpdateCommand>, IAttributeDefinitionService
{
    private readonly IAttributeDefinitionRepository _attributeDefinitionRepository;
    private readonly IAttributeValueRepository _attributeValueRepository;

    public AttributeDefinitionService(
        IAttributeDefinitionRepository attributeDefinitionRepository,
        IAttributeValueRepository attributeValueRepository)
        : base(attributeDefinitionRepository)
    {
        _attributeDefinitionRepository = attributeDefinitionRepository;
        _attributeValueRepository = attributeValueRepository;
    }

    public async Task<AttributeDefinitionDto?> GetWithPredefinedValues(Guid id)
    {
        var entity = await _attributeDefinitionRepository.GetWithPredefinedValuesAsync(id);
        return new AttributeDefinitionDto
        {
            Id = entity.Id,
            IsActive = entity.IsActive,
            Name = entity.Name,
            NameSecondLanguage = entity.NameSecondLanguage,
            PredefinedValues = entity.PredefinedValues.Select(e=>e.Adapt<AttributeValueDto>()).ToList()
        };
    }

    public async Task<List<AttributeDefinitionDto>> GetActiveAttributeDefinitions()
    {
        var entities = await _attributeDefinitionRepository.GetActiveAttributeDefinitionsAsync();
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
