using System.Text.RegularExpressions;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;
using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Product read queries.
/// </summary>
public class ProductQueries(
    IProductRepository repository,
    IBaseRepository<ProductCode> codeRepository)
    : BaseTreeSettingRepositoryQueries<Product>(repository),
      IProductQueries
{
    public async Task<ApiResponse<List<ProductDto>>> GetAll()
    {
        try
        {
            var result = await repository.GetDtos();
            return new ApiResponse<List<ProductDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.ToList()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<ProductDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<ProductDto?>> GetById(Guid id)
    {
        try
        {
            var result = await repository.GetDtoById(id);
            return new ApiResponse<ProductDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<string>> GetNextCode(Guid? parentId = null)
    {
        var parent = await codeRepository.GetQuery()
            .Where(e => e.ProductId == parentId && e.CodeType == ProductCodeType.Code)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.Code)
            .FirstOrDefaultAsync();

        var lastSibling = await repository.GetQuery()
            .Where(e => e.ParentId == parentId)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();

        var lastSiblingCode = lastSibling == null
            ? string.Empty
            : await codeRepository.GetQuery()
                .Where(e => e.ProductId == lastSibling.Id && e.CodeType == ProductCodeType.Code)
                .Select(e => e.Code)
                .FirstOrDefaultAsync() ?? string.Empty;

        var code = GetNextChildCode(lastSiblingCode, parent ?? string.Empty);
        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = code
        };
    }

    private static string GetNextChildCode(string lastSiblingCode, string parentCode)
    {
        if (string.IsNullOrEmpty(lastSiblingCode))
            return parentCode + "001";

        var match = Regex.Match(lastSiblingCode, @"(\d+)$");
        if (match.Success)
        {
            var numberPart = match.Groups[1].Value;
            var prefix = lastSiblingCode[..^numberPart.Length];
            var nextNumber = int.Parse(numberPart) + 1;
            return prefix + nextNumber.ToString("D" + numberPart.Length);
        }

        return lastSiblingCode + "001";
    }
}
