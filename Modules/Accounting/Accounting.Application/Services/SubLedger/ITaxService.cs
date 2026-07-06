using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Tax default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public interface ITaxService
{
    Task<ApiResponse<TaxCreateCommand>> GetNextSubLeadgers(Guid? parentId);
}
