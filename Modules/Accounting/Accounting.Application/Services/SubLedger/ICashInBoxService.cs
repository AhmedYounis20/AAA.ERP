using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Cash-in-box default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface ICashInBoxService
{
    Task<ApiResponse<CashInBoxCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
