using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Taxes;

public record TaxDeleteCommand(Guid Id) : ICommand<ApiResponse<Tax>>, IEntityDeleteCommand;
