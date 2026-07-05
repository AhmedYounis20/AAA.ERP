using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.PaymentEntries;

/// <summary>Deletes a payment entry.</summary>
public record PaymentEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
