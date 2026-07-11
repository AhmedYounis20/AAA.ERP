using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.JournalEntries;

/// <summary>Deletes a journal entry.</summary>
public record JournalEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
