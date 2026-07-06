using Accounting.Application.Commands.Banks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Bank default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface IBankService
{
    Task<ApiResponse<BankCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
