using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface IAccountGuideReadService
{
    Task<ApiResponse<IEnumerable<AccountGuide>>> GetAll();
    Task<ApiResponse<PaginatedResult<AccountGuide>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<AccountGuide>> GetById(Guid id);
}
