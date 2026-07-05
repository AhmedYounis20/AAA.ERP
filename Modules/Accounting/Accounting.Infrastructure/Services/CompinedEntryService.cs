using AAA.ERP.OutputDtos;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Commands.CompinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class CompinedEntryService(IComplexEntryService entryService, IEntryRepository entryRepository)
            : BaseService<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>(entryRepository), ICompinedEntryService
{
    public override async Task<ApiResponse<Entry>> Create(CompinedEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Compined;
        return await entryService.Create(complexEntry, isValidate);
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
    => await entryService.GetComplexEntries(EntryType.Compined);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
    => await entryService.GetComplexEntryById(id, EntryType.Compined);

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await entryService.GetEntryNumber(dateTime);
    }

    public override async Task<ApiResponse<Entry>> Update(CompinedEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await entryService.Update(complexEntry, isValidate);
    }
}