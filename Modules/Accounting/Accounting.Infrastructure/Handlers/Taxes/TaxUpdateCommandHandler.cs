using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Entities;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Handlers.Taxes;

public class TaxUpdateCommandHandler(ITaxRepository repository)
    : ICommandHandler<TaxUpdateCommand, ApiResponse<Tax>>
{
    public async Task<ApiResponse<Tax>> Handle(TaxUpdateCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await repository.Get(command.Id);
            if (entity is null)
            {
                return new ApiResponse<Tax>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
                };
            }

            if (entity.NodeType != command.NodeType)
            {
                return new ApiResponse<Tax>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.CannotChangeNodeType }]
                };
            }

            var updated = command.Adapt<Tax>();
            if (entity.ChartOfAccount is not null)
            {
                updated.ChartOfAccountId = entity.ChartOfAccountId;
                updated.ChartOfAccount = entity.ChartOfAccount;
                updated.ChartOfAccount.Name = command.Name;
                updated.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
            }

            await repository.Update(updated);

            return new ApiResponse<Tax>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Tax>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }
}
