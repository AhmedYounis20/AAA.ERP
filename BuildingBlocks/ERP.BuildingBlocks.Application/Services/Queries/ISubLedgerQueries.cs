using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.Services.Queries;

/// <summary>
/// Read-side contract for sub-ledger tree entities (hybrid CQRS — no writes).
/// </summary>
public interface ISubLedgerQueries<TEntity, TCreateCommand> : ITreeSettingQueries<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseTreeSettingCreateCommand<TEntity>
{
    Task<ApiResponse<TCreateCommand>> GetNextSubLedgers(Guid? parentId);
}
