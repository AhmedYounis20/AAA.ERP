using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Persistence.Repositories;
using Shared;

namespace Accounting.Infrastructure.Handlers.SubLedger;

/// <summary>
/// Generic delete handler for sub-ledger entities. Mirrors <c>SubLeadgerService.Delete</c>.
/// </summary>
public abstract class BaseSubLedgerDeleteHandler<TEntity, TDeleteCommand>(
    IBaseSubLeadgerRepository<TEntity> repository,
    IUnitOfWork unitOfWork,
    IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingDeleteHandler<TEntity, TDeleteCommand>(repository)
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TDeleteCommand : ICommand<ApiResponse<TEntity>>, IEntityDeleteCommand
{
    public override async Task<ApiResponse<TEntity>> Handle(
        TDeleteCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var validation = await ValidateDeleteAsync(command.Id, cancellationToken);
            if (!validation.IsValid)
            {
                return new ApiResponse<TEntity>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validation.Errors.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validation.Entity;
            if (entity is null)
            {
                return new ApiResponse<TEntity>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = null
                };
            }

            var chartOfAccountId = entity.ChartOfAccountId;

            await unitOfWork.BeginTransactionAsync();
            try
            {
                entity.ChartOfAccount = null;
                await Repository.Delete(entity);

                if (chartOfAccountId.HasValue)
                    await chartOfAccountRepository.Delete(chartOfAccountId.Value);

                await unitOfWork.CommitAsync();

                return new ApiResponse<TEntity>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = entity
                };
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<TEntity>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = [new MessageTemplate { MessageKey = ex.Message }]
                };
            }
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
