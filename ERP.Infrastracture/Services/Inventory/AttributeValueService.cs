using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Infrastracture.Services.Inventory;

public class AttributeValueService : BaseSettingService<AttributeValue, AttributeValueCreateCommand, AttributeValueUpdateCommand>, IAttributeValueService
{
    private readonly IAttributeValueRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AttributeValueService(IAttributeValueRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos()
    {
        try
        {
            var result = await _repository.GetQuery().Include(e=>e.AttributeDefinition).Select(e=> new AttributeValueDto { 
                Name = e.Name,
                NameSecondLanguage = e.NameSecondLanguage,
                AttributeDefinitionName = e.AttributeDefinition != null ? e.AttributeDefinition.Name : string.Empty,
                AttributeDefinitionNameSecondLanguange = e.AttributeDefinition != null ? e.AttributeDefinition.NameSecondLanguage : string.Empty,
                AttributeDefinitionId = e.AttributeDefinitionId,
                Id = e.Id,
                IsActive = e.IsActive,
                SortOrder = e.SortOrder
            }).ToListAsync();
            return new ApiResponse<List<AttributeValueDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.ToList()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id)
    {
        try
        {
            var result = (await _repository.Get(id)).Adapt<AttributeValueDto>();
            return new ApiResponse<AttributeValueDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AttributeValueDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId)
    {
        try
        {
            var result = await _repository.GetByAttributeDefinitionIdAsync(attributeId);
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.Adapt<List<AttributeValueDto>>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValuesByAttributeDefenitionId(Guid attributeId)
    {
        try
        {
            var result = (await _repository.GetActiveAttributeValuesAsync()).Where(e=>e.AttributeDefinitionId == attributeId);
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.Adapt<List<AttributeValueDto>>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByValue(string value)
    {
        try
        {
            var result = await _repository.GetQuery().Where(e=>e.Name == value ||  e.NameSecondLanguage == value).ToListAsync();

            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.Adapt<List<AttributeValueDto>>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues()
    {
        try
        {
            var result = (await _repository.Get()).Adapt<List<AttributeValueDto>>();
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<AttributeValueDto>> CreateAttributeValue(AttributeValueCreateCommand command)
    {
        try
        {
            var attributeValue = new AttributeValue
            {
                Name = command.Name,
                NameSecondLanguage = command.NameSecondLanguage,
                SortOrder = command.SortOrder,
                IsActive = command.IsActive,
                AttributeDefinitionId = command.AttributeDefinitionId
            };

            var result = await _repository.Add(attributeValue);
            await _unitOfWork.SaveChanges();

            var attributeValueDto = (await _repository.Get(result.Id)).Adapt<AttributeValueDto>();
            return new ApiResponse<AttributeValueDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = attributeValueDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AttributeValueDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<AttributeValueDto>> UpdateAttributeValue(AttributeValueUpdateCommand command)
    {
        try
        {
            var attributeValue = await _repository.Get(command.Id);
            if (attributeValue == null)
            {
                return new ApiResponse<AttributeValueDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "AttributeValueNotFound" } }
                };
            }

            attributeValue.Name = command.Name;
            attributeValue.NameSecondLanguage = command.NameSecondLanguage;
            attributeValue.SortOrder = command.SortOrder;
            attributeValue.IsActive = command.IsActive;
            attributeValue.AttributeDefinitionId = command.AttributeDefinitionId;

            await _repository.Update(attributeValue);
            await _unitOfWork.SaveChanges();

            var attributeValueDto = (await _repository.Get(attributeValue.Id)).Adapt<AttributeValueDto>();
            return new ApiResponse<AttributeValueDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = attributeValueDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AttributeValueDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteAttributeValue(Guid id)
    {
        try
        {
            var attributeValue = await _repository.Get(id);
            if (attributeValue == null)
            {
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = "AttributeValueNotFound" } }
                };
            }

            await _repository.Delete(attributeValue);
            await _unitOfWork.SaveChanges();

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }
}

