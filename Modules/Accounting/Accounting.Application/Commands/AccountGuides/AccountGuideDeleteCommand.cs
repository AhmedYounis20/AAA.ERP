using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.AccountGuides;

public record AccountGuideDeleteCommand(Guid Id) : ICommand<ApiResponse<AccountGuide>>;
