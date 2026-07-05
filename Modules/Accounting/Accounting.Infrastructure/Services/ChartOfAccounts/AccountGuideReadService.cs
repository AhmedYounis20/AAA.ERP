using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class AccountGuideReadService(IAccountGuideService accountGuideService) : IAccountGuideReadService
{
    public Task<ApiResponse<IEnumerable<AccountGuide>>> GetAll() => accountGuideService.ReadAll();
    public Task<ApiResponse<PaginatedResult<AccountGuide>>> GetPaginated(SettingFilterDto filter, CancellationToken cancellationToken = default)
        => accountGuideService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<AccountGuide>> GetById(Guid id) => accountGuideService.ReadById(id);
}
