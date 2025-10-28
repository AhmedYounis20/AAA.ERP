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
    private readonly IUnitOfWork _unitOfWork;

    public AttributeDefinitionService(
        IAttributeDefinitionRepository attributeDefinitionRepository,
        IAttributeValueRepository attributeValueRepository,
        IUnitOfWork unitOfWork)
        : base(attributeDefinitionRepository)
    {
        _attributeDefinitionRepository = attributeDefinitionRepository;
        _attributeValueRepository = attributeValueRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<ApiResponse<AttributeDefinition>> Create(AttributeDefinitionCreateCommand command, bool isValidate = true)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // First, create the attribute definition using the base implementation
            var createDefinitionResponse = await base.Create(command, isValidate);

            // If creation failed, rollback and return
            if (!createDefinitionResponse.IsSuccess || createDefinitionResponse.Result is null)
            {
                await _unitOfWork.RollbackAsync();
                return createDefinitionResponse;
            }

            // If no predefined values, commit and return
            if (command.PredefinedValues is null || command.PredefinedValues.Count == 0)
            {
                await _unitOfWork.CommitAsync();
                return createDefinitionResponse;
            }

            // Attach incoming predefined values to the newly created definition
            var definitionId = createDefinitionResponse.Result.Id;

            var valuesToCreate = command.PredefinedValues
                .Where(v => !string.IsNullOrWhiteSpace(v.Name) || !string.IsNullOrWhiteSpace(v.NameSecondLanguage))
                .Select(v => new AttributeValue
                {
                    Name = v.Name,
                    NameSecondLanguage = v.NameSecondLanguage,
                    IsActive = v.IsActive,
                    SortOrder = v.SortOrder,
                    AttributeDefinitionId = definitionId
                })
                .ToList();

            if (valuesToCreate.Count > 0)
            {
                await _attributeValueRepository.Add(valuesToCreate);
            }

            await _unitOfWork.CommitAsync();
            return createDefinitionResponse;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { $"Failed to create attribute definition with values: {ex.Message}" }
            };
        }
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

    public override async Task<ApiResponse<AttributeDefinition>> Update(AttributeDefinitionUpdateCommand command, bool isValidate = true)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            // Update the definition using base logic first
            var updateDefinitionResponse = await base.Update(command, isValidate);
            if (!updateDefinitionResponse.IsSuccess || updateDefinitionResponse.Result is null)
            {
                await _unitOfWork.RollbackAsync();
                return updateDefinitionResponse;
            }

            // If no predefined values provided, commit and return
            if (command.PredefinedValues is null)
            {
                await _unitOfWork.CommitAsync();
                return updateDefinitionResponse;
            }

            var definitionId = updateDefinitionResponse.Result.Id;

            // Load existing values
            var existingValues = await _attributeValueRepository.GetByAttributeDefinitionIdAsync(definitionId);
            var existingById = existingValues.ToDictionary(v => v.Id, v => v);

            // Prepare create and update sets
            var toCreate = new List<AttributeValue>();
            var incomingIds = new HashSet<Guid>();

            foreach (var v in command.PredefinedValues)
            {
                if (v.Id == Guid.Empty || !existingById.ContainsKey(v.Id))
                {
                    // New value
                    if (string.IsNullOrWhiteSpace(v.Name) && string.IsNullOrWhiteSpace(v.NameSecondLanguage))
                        continue;

                    toCreate.Add(new AttributeValue
                    {
                        Name = v.Name,
                        NameSecondLanguage = v.NameSecondLanguage,
                        IsActive = v.IsActive,
                        SortOrder = v.SortOrder,
                        AttributeDefinitionId = definitionId
                    });
                }
                else
                {
                    // Update existing
                    incomingIds.Add(v.Id);
                    var ex = existingById[v.Id];
                    ex.Name = v.Name;
                    ex.NameSecondLanguage = v.NameSecondLanguage;
                    ex.IsActive = v.IsActive;
                    ex.SortOrder = v.SortOrder;
                    ex.AttributeDefinitionId = definitionId;
                    await _attributeValueRepository.Update(ex);
                }
            }

            if (toCreate.Count > 0)
            {
                await _attributeValueRepository.Add(toCreate);
            }

            // Delete values that are no longer present
            var idsToKeep = new HashSet<Guid>(incomingIds.Where(id => id != Guid.Empty));
            var toDelete = existingValues.Where(ev => !idsToKeep.Contains(ev.Id)).ToList();
            if (toDelete.Count > 0)
            {
                await _attributeValueRepository.Delete(toDelete);
            }

            await _unitOfWork.CommitAsync();
            return updateDefinitionResponse;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = new List<string> { $"Failed to update attribute definition with values: {ex.Message}" }
            };
        }
    }
}
