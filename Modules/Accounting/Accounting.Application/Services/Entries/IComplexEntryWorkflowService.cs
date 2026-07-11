using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for complex entry CRUD orchestration (Category C).
/// </summary>
public interface IComplexEntryWorkflowService
{
    Task<ApiResponse<Entry>> Create(ComplexEntryCreateCommand command);

    Task<ApiResponse<Entry>> Update(ComplexEntryUpdateCommand command);
}
