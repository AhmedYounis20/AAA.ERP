using System.Net;
using ERP.BuildingBlocks.Persistence.Repositories;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.Infrastructure.Repositories;

public sealed class TransactionCoordinator : ITransactionCoordinator
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionCoordinator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<T>> ExecuteAsync<T>(
        Func<CancellationToken, Task<ApiResponse<T>>> action,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await action(cancellationToken);
            if (result.IsSuccess)
                await _unitOfWork.CommitAsync();
            else
                await _unitOfWork.RollbackAsync();

            return result;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
