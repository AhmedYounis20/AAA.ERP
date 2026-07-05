using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class RoleReadService(IRoleService roleService) : IRoleReadService
{
    public Task<ApiResponse<IEnumerable<Role>>> GetAll() => roleService.ReadAll();
    public Task<ApiResponse<PaginatedResult<Role>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default)
        => roleService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<Role>> GetById(Guid id) => roleService.ReadById(id);
}
