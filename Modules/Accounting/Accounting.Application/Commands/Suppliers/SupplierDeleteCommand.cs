using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Suppliers;

/// <summary>Deletes a supplier sub-ledger record.</summary>
public record SupplierDeleteCommand(Guid Id) : ICommand<ApiResponse<Supplier>>;
