using Accounting.Application.Commands.Suppliers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for supplier GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISupplierQueries : ITreeSettingQueries<Supplier>
{
    Task<ApiResponse<SupplierCreateCommand>> GetNextSubLedgers(Guid? parentId);
}
