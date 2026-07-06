using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Chart-of-account domain helpers. CRUD writes are handled by CQRS handlers.
/// </summary>
public class ChartOfAccountService(IChartOfAccountRepository repository) : IChartOfAccountService
{
    public Task<string> GenerateNewCodeForChild(Guid? parentId)
        => repository.GenerateNewCodeForChild(parentId);

    public async Task<ApiResponse<ChartOfAccountInputModel>> NextAccountDefaultData(Guid? parentId)
    {
        try
        {
            ChartOfAccount? parent = parentId is not null ? await repository.Get(parentId ?? Guid.Empty) : null;
            var inputModel = new ChartOfAccountInputModel
            {
                parentId = parentId,
                AccountNature = parent?.AccountNature ?? AccountNature.Debit,
                AccountGuidId = parent?.AccountGuidId ?? Guid.Empty,
                Code = await GenerateNewCodeForChild(parentId),
                IsPostedAccount = parent?.IsPostedAccount ?? false,
                IsStopDealing = parent?.IsStopDealing ?? false,
                IsDepreciable = false,
                IsActiveAccount = true,
            };

            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = inputModel
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
            };
        }
    }
}
