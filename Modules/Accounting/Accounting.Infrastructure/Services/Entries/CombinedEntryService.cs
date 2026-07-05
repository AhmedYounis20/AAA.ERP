using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Commands.CombinedEntries;
using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class CombinedEntryService(IComplexEntryService entryService, IEntryRepository entryRepository)
            : BaseService<Entry, CombinedEntryCreateCommand, CombinedEntryUpdateCommand>(entryRepository), ICombinedEntryService
{
    public override async Task<ApiResponse<Entry>> Create(CombinedEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Combined;
        return await entryService.Create(complexEntry, isValidate);
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
    => await entryService.GetComplexEntries(EntryType.Combined);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
    => await entryService.GetComplexEntryById(id, EntryType.Combined);

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await entryService.GetEntryNumber(dateTime);
    }

    public override async Task<ApiResponse<Entry>> Update(CombinedEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await entryService.Update(complexEntry, isValidate);
    }
}