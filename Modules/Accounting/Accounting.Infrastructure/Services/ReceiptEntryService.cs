using AAA.ERP.OutputDtos;
using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Services;
using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class ReceiptEntryService(IComplexEntryService entryService, IEntryRepository entryRepository)
    : BaseService<Entry, ReceiptEntryCreateCommand, ReceiptEntryUpdateCommand>(entryRepository), IReceiptEntryService
{
    public override async Task<ApiResponse<Entry>> Create(ReceiptEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Receipt;
        return await entryService.Create(complexEntry, isValidate);
    }

    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
        => entryService.GetComplexEntries(EntryType.Receipt);

    public Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
        => entryService.GetComplexEntryById(id, EntryType.Receipt);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);

    public override async Task<ApiResponse<Entry>> Update(ReceiptEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await entryService.Update(complexEntry, isValidate);
    }
}
