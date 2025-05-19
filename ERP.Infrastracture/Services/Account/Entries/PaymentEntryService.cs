using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Commands.Account.Entries.PaymentEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Infrastracture.Services.Account.Entries;

public class PaymentEntryService(IComplexEntryService _entryService, IEntryRepository _entryRepository)
            : BaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>(_entryRepository), IPaymentEntryService
{
    public override async Task<ApiResponse<Entry>> Create(PaymentEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Payment;
        return await _entryService.Create(complexEntry, isValidate);
    }

    public async Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
    => await _entryService.GetComplexEntries(EntryType.Payment);

    public async Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
    => await _entryService.GetComplexEntryById(id, EntryType.Payment);

    public async Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
    {
        return await _entryService.GetEntryNumber(dateTime);
    }

    public override async Task<ApiResponse<Entry>> Update(PaymentEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await _entryService.Update(complexEntry, isValidate);
    }
}