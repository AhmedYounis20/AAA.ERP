using Accounting.Application.Services;
using ERP.Infrastructure.Utilities;

namespace ERP.Host.Startup;

internal sealed class SubLedgerCoaSeedImporter(ImportDataToSeed importDataToSeed) : ISubLedgerCoaSeedImporter
{
    public Task ImportAccountSeedAsync(CancellationToken cancellationToken = default)
        => importDataToSeed.Import("account");
}
