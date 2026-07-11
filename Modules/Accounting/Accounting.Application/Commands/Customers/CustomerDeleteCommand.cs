using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Customers;

public record CustomerDeleteCommand(Guid Id) : ICommand<ApiResponse<Customer>>, IEntityDeleteCommand;
