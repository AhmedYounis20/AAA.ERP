using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.CostCenters;

public record CostCenterDeleteCommand(Guid Id) : ICommand<ApiResponse<CostCenter>>;
