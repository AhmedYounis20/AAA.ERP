using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using Accounting.Infrastructure.Handlers.SubLedger;

namespace Accounting.Infrastructure.Handlers.Taxes;

public sealed class TaxUpdateCommandHandler(ITaxRepository repository)
    : BaseSubLedgerUpdateHandler<Tax, TaxUpdateCommand>(repository)
{
    protected override bool ShouldValidateBusinessRules => true;

    protected override async Task<CrudEntityValidationResult<Tax>> ValidateUpdateAsync(
        TaxUpdateCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await TreeSettingRepository.Get(command.Id);
        if (entity is null)
        {
            return CrudEntityValidationResult<Tax>.Failure(
                new[] { ErrorMessages.RecordNotFound });
        }

        if (entity.NodeType != command.NodeType)
        {
            return CrudEntityValidationResult<Tax>.Failure(
                new[] { ErrorMessages.CannotChangeNodeType },
                entity);
        }

        return CrudEntityValidationResult<Tax>.Success(entity);
    }
}
