using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Read-side attribute value operations (DTO projections and filtered lists).
/// </summary>
public class AttributeValueService(IAttributeValueRepository repository) : IAttributeValueService
{
    public async Task<ApiResponse<List<AttributeValueDto>>> GetAttributeValueDtos()
    {
        try
        {
            var result = await repository.GetQuery().Include(e => e.AttributeDefinition).Select(e => new AttributeValueDto
            {
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
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<AttributeValueDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<AttributeValueDto?>> GetAttributeValueDtoById(Guid id)
    {
        try
        {
            var result = (await repository.Get(id)).Adapt<AttributeValueDto>();
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
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetAttributeValuesByAttributeDefenitionId(Guid attributeId)
    {
        try
        {
            var result = await repository.GetByAttributeDefinitionIdAsync(attributeId);
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
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<AttributeValueDto>>> GetActiveAttributeValues()
    {
        try
        {
            var result = (await repository.Get()).Adapt<List<AttributeValueDto>>();
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
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
