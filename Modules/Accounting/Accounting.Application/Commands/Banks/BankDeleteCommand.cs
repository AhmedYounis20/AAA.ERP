using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Banks;

/// <summary>Deletes a bank sub-ledger record.</summary>
public record BankDeleteCommand(Guid Id) : ICommand<ApiResponse<Bank>>, IEntityDeleteCommand;
