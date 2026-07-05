using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Customers;

/// <summary>Deletes a customer sub-ledger record.</summary>
public record CustomerDeleteCommand(Guid Id) : ICommand<ApiResponse<Customer>>;
