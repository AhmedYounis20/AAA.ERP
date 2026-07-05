using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface ICollectionBookReadService
{
    Task<ApiResponse<IEnumerable<CollectionBook>>> GetAll();
    Task<ApiResponse<PaginatedResult<CollectionBook>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<CollectionBook>> GetById(Guid id);
}
