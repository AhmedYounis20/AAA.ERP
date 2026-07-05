using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Branches;

/// <summary>Deletes a branch sub-ledger record.</summary>
public record BranchDeleteCommand(Guid Id) : ICommand<ApiResponse<Branch>>;
