using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.CombinedEntries;

/// <summary>Deletes a combined entry.</summary>
public record CombinedEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
