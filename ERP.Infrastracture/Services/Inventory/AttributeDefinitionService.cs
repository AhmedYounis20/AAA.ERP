using AutoMapper;
using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Services.Inventory;

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
