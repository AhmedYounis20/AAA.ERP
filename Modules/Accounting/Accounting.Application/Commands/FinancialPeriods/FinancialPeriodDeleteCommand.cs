using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.FinancialPeriods;

/// <summary>Deletes a financial period.</summary>
public record FinancialPeriodDeleteCommand(Guid Id) : ICommand<ApiResponse<FinancialPeriod>>;
