using AAA.ERP.OutputDtos;
using Domain.Account.Commands.Entries;
using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces.Entries;
using Mapster;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation.Entries;

public class ReceiptEntryService(IComplexEntryService _entryService, IEntryRepository _entryRepository)
            : BaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>(_entryRepository), IReceiptEntryService
{
    public override async Task<ApiResponse<Entry>> Create(ReceiptEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Receipt;
        return await _entryService.Create(complexEntry, isValidate);
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
    => await _entryService.GetComplexEntries(EntryType.Receipt);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
    => await _entryService.GetComplexEntryById(id,EntryType.Receipt);


    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }

    public override async Task<ApiResponse<Entry>> Update(ReceiptEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await _entryService.Update(complexEntry, isValidate);
    }
}