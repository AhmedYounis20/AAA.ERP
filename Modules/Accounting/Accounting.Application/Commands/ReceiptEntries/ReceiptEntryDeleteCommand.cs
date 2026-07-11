using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.ReceiptEntries;

/// <summary>Deletes a receipt entry.</summary>
public record ReceiptEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
