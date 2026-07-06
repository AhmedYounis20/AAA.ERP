using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Suppliers;

public record SupplierDeleteCommand(Guid Id) : ICommand<ApiResponse<Supplier>>, IEntityDeleteCommand;
