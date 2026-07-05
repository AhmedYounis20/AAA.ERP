using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.OpeningEntries;

/// <summary>Deletes an opening entry.</summary>
public record OpeningEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
