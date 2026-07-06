using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for core entry CRUD orchestration (Category C).
/// </summary>
public interface IEntryWorkflowService
{
    Task<ApiResponse<Entry>> Create(EntryCreateCommand command);

    Task<ApiResponse<Entry>> Update(EntryUpdateCommand command);

    Task<ApiResponse<Entry>> Delete(Guid id);
}
