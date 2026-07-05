using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface IRoleReadService
{
    Task<ApiResponse<IEnumerable<Role>>> GetAll();
    Task<ApiResponse<PaginatedResult<Role>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<Role>> GetById(Guid id);
}
