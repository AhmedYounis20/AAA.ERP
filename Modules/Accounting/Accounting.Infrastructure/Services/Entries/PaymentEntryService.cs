using Accounting.Contracts.Dtos;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Commands.Entries;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Entries;
using ERP.Infrastructure.Services.BaseServices;

namespace Accounting.Infrastructure.Services;

public class PaymentEntryService(IComplexEntryService entryService, IEntryRepository entryRepository)
    : BaseService<Entry, PaymentEntryCreateCommand, PaymentEntryUpdateCommand>(entryRepository), IPaymentEntryService
{
    public override async Task<ApiResponse<Entry>> Create(PaymentEntryCreateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryCreateCommand>();
        complexEntry.Type = EntryType.Payment;
        return await entryService.Create(complexEntry, isValidate);
    }

    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetComplexEntries()
        => entryService.GetComplexEntries(EntryType.Payment);

    public Task<ApiResponse<ComplexEntryDto>> GetComplexEntryById(Guid id)
        => entryService.GetComplexEntryById(id, EntryType.Payment);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);

    public override async Task<ApiResponse<Entry>> Update(PaymentEntryUpdateCommand entity, bool isValidate = true)
    {
        var complexEntry = entity.Adapt<ComplexEntryUpdateCommand>();
        return await entryService.Update(complexEntry, isValidate);
    }
}
