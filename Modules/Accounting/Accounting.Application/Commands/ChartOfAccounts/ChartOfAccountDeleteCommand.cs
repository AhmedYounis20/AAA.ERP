using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.ChartOfAccounts;

/// <summary>Deletes a chart-of-account node.</summary>
public record ChartOfAccountDeleteCommand(Guid Id) : ICommand<ApiResponse<ChartOfAccount>>, IEntityDeleteCommand;
