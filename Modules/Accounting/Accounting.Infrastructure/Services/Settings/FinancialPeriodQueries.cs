using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.FinancialPeriods;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Financial period read queries.
/// </summary>
public class FinancialPeriodQueries(IFinancialPeriodRepository repository) : IFinancialPeriodQueries
{
    private readonly BaseEntityRepositoryQueries<FinancialPeriod> _entityQueries = new(repository);

    public async Task<ApiResponse<List<FinancialPeriodDto>>> GetAll()
    {
        try
        {
            var entities = await repository.GetDtos();
            return new ApiResponse<List<FinancialPeriodDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<FinancialPeriodDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public Task<ApiResponse<PaginatedResult<FinancialPeriod>>> GetPaginated(
        FinancialPeriodFilterDto filter,
        CancellationToken cancellationToken = default)
        => _entityQueries.GetPaginated(filter, cancellationToken);

    public async Task<ApiResponse<FinancialPeriodDto?>> GetById(Guid id)
    {
        try
        {
            var entity = await repository.GetDto(id);
            return new ApiResponse<FinancialPeriodDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriodDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata()
    {
        try
        {
            var entity = await repository.GetNextDefaultdata();
            return new ApiResponse<FinancialPeriodDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriodDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriod>> GetCurrentFinancialPeriod()
    {
        var currentFinancialPeriod = await repository.GetCurrentFinancialPeroid();
        if (currentFinancialPeriod is null)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.FinancialPeriodNotFound }]
            };
        }

        return new ApiResponse<FinancialPeriod>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = currentFinancialPeriod
        };
    }
}
