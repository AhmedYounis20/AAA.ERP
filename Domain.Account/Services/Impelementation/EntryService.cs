using Domain.Account.Commands.Currencies;
using Domain.Account.Commands.GLSettings;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class EntryService : BaseService<Entry,EntryCreateCommand,EntryUpdateCommand>,IEntryService
{
    public EntryService(IEntryRepository repository) : base(repository)
    {
    }
}