using Account.Domain.Commands.FinancialPeriods;
using Account.Domain.DTOs;
using Account.Domain.Models.Entities.FinancialPeriods;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Account.Application.Services;

public interface IFinancialPeriodService : IBaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>
{

    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
    Task<ApiResponse<List<FinancialPeriodDto>>> GetDtos();
    Task<ApiResponse<FinancialPeriodDto?>> GetDto(Guid id);
    Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata();
}