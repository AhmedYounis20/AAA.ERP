using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.Responses;
using Shared;

namespace Accounting.Application.Commands.Entries;

/// <summary>Deletes a complex accounting entry.</summary>
public record ComplexEntryDeleteCommand(Guid Id) : ICommand<ApiResponse<Entry>>;
