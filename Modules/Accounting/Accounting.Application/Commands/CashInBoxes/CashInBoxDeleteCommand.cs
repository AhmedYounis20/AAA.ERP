using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.CashInBoxes;

public record CashInBoxDeleteCommand(Guid Id) : ICommand<ApiResponse<CashInBox>>, IEntityDeleteCommand;
