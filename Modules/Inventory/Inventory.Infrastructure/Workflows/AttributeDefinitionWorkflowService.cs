using Inventory.Application.Commands.AttributeDefinitions;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Attribute definition write orchestration (transactional create/update with predefined values).
/// </summary>
public class AttributeDefinitionWorkflowService(
    IAttributeDefinitionRepository attributeDefinitionRepository,
    IAttributeValueRepository attributeValueRepository,
    IUnitOfWork unitOfWork) : IAttributeDefinitionWorkflowService
{
    public async Task<ApiResponse<AttributeDefinition>> Create(AttributeDefinitionCreateCommand command)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var predefinedValues = command.PredefinedValues;
            command.PredefinedValues = [];

            var createDefinitionResponse = await CreateDefinitionAsync(command);
            if (!createDefinitionResponse.IsSuccess || createDefinitionResponse.Result is null)
            {
                await unitOfWork.RollbackAsync();
                return createDefinitionResponse;
            }

            if (predefinedValues is null || predefinedValues.Count == 0)
            {
                await unitOfWork.CommitAsync();
                return createDefinitionResponse;
            }

            var definitionId = createDefinitionResponse.Result.Id;

            var valuesToCreate = predefinedValues
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
                await attributeValueRepository.Add(valuesToCreate);

            await unitOfWork.CommitAsync();
            return createDefinitionResponse;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors =
                [
                    new MessageTemplate
                    {
                        MessageKey = $"Failed to create attribute definition with values: {ex.Message}"
                    }
                ]
            };
        }
    }

    public async Task<ApiResponse<AttributeDefinition>> Update(AttributeDefinitionUpdateCommand command)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync();
            var predefinedValues = command.PredefinedValues;
            command.PredefinedValues = [];

            var updateDefinitionResponse = await UpdateDefinitionAsync(command);
            if (!updateDefinitionResponse.IsSuccess || updateDefinitionResponse.Result is null)
            {
                await unitOfWork.RollbackAsync();
                return updateDefinitionResponse;
            }

            if (predefinedValues is null)
            {
                await unitOfWork.CommitAsync();
                return updateDefinitionResponse;
            }

            var definitionId = updateDefinitionResponse.Result.Id;

            var existingValues = await attributeValueRepository.GetByAttributeDefinitionIdAsync(definitionId);
            var existingById = existingValues.ToDictionary(v => v.Id, v => v);

            var toCreate = new List<AttributeValue>();
            var incomingIds = new HashSet<Guid>();

            foreach (var v in predefinedValues)
            {
                if (!v.Id.HasValue || !existingById.ContainsKey(v.Id.Value))
                {
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
                    incomingIds.Add(v.Id.Value);
                    var ex = existingById[v.Id.Value];
                    ex.Name = v.Name;
                    ex.NameSecondLanguage = v.NameSecondLanguage;
                    ex.IsActive = v.IsActive;
                    ex.SortOrder = v.SortOrder;
                    ex.AttributeDefinitionId = definitionId;
                    await attributeValueRepository.Update(ex);
                }
            }

            if (toCreate.Count > 0)
                await attributeValueRepository.Add(toCreate);

            var idsToKeep = new HashSet<Guid>(incomingIds.Where(id => id != Guid.Empty));
            var toDelete = existingValues.Where(ev => !idsToKeep.Contains(ev.Id)).ToList();
            if (toDelete.Count > 0)
                await attributeValueRepository.Delete(toDelete);

            await unitOfWork.CommitAsync();
            return updateDefinitionResponse;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors =
                [
                    new MessageTemplate
                    {
                        MessageKey = $"Failed to update attribute definition with values: {ex.Message}"
                    }
                ]
            };
        }
    }

    public async Task<ApiResponse<AttributeDefinition>> Delete(Guid id)
    {
        try
        {
            var validationResult = await ValidateDelete(id);
            if (!validationResult.isValid)
            {
                return new ApiResponse<AttributeDefinition>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
                await attributeDefinitionRepository.Delete(entity);

            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<ApiResponse<AttributeDefinition>> CreateDefinitionAsync(AttributeDefinitionCreateCommand command)
    {
        var validationResult = await ValidateCreate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        var entity = command.Adapt<AttributeDefinition>();
        await attributeDefinitionRepository.Add(entity);
        return new ApiResponse<AttributeDefinition>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = entity
        };
    }

    private async Task<ApiResponse<AttributeDefinition>> UpdateDefinitionAsync(AttributeDefinitionUpdateCommand command)
    {
        var validationResult = await ValidateUpdate(command);
        if (!validationResult.isValid)
        {
            return new ApiResponse<AttributeDefinition>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        var entity = command.Adapt<AttributeDefinition>();
        entity.ModifiedAt = DateTime.Now;
        await attributeDefinitionRepository.Update(entity);
        return new ApiResponse<AttributeDefinition>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = entity
        };
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(AttributeDefinitionCreateCommand command)
    {
        var isValid = true;
        var errors = new List<string>();

        var existedEntity = await attributeDefinitionRepository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is not null)
        {
            isValid = false;
            if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
                errors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
                errors.Add("WithSameNameSecondLanguageIsExisted");
        }

        return (isValid, errors);
    }

    private async Task<(bool isValid, List<string> errors, AttributeDefinition? entity)> ValidateUpdate(
        AttributeDefinitionUpdateCommand command)
    {
        var isValid = true;
        var errors = new List<string>();

        var oldEntity = await attributeDefinitionRepository.Get(command.Id);
        var existedEntity = await attributeDefinitionRepository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is not null && existedEntity.Id != command.Id)
        {
            isValid = false;
            if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
                errors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
                errors.Add("WithSameNameSecondLanguageIsExisted");
        }

        return (isValid, errors, oldEntity);
    }

    private async Task<(bool isValid, List<string> errors, AttributeDefinition? entity)> ValidateDelete(Guid id)
    {
        var entity = await attributeDefinitionRepository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }
}
