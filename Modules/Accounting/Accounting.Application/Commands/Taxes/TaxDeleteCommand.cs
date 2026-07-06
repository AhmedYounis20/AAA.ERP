using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Taxes;

/// <summary>Deletes a tax sub-ledger record.</summary>
public record TaxDeleteCommand(Guid Id) : ICommand<ApiResponse<Tax>>;
