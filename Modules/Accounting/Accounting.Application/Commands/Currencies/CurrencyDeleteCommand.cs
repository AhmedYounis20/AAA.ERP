using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Currencies;

public record CurrencyDeleteCommand(Guid Id) : ICommand<ApiResponse<Currency>>;
