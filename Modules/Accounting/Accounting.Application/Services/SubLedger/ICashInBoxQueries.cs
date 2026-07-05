using Accounting.Application.Commands.CashInBoxes;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for cash-in-box GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ICashInBoxQueries : ITreeSettingQueries<CashInBox>
{
    Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLedgers(Guid? parentId);
}
