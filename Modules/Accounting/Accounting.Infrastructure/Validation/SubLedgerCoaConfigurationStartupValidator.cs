using Accounting.Application.Services;
using ERP.BuildingBlocks.Application.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Accounting.Infrastructure.Validation;

/// <summary>
/// Ensures sub-ledger base COA configuration exists before the application accepts traffic.
/// Optionally imports account seed data when configuration is missing (development bootstrap).
/// </summary>
public sealed class SubLedgerCoaConfigurationStartupValidator(
    IServiceScopeFactory serviceScopeFactory,
    IConfiguration configuration,
    IHostEnvironment hostEnvironment,
    ILogger<SubLedgerCoaConfigurationStartupValidator> logger) : IHostedService
{
    private const string ValidateEnabledConfigKey = "Accounting:SubLedger:ValidateConfigurationOnStartup";
    private const string ImportSeedConfigKey = "Accounting:SubLedger:ImportSeedIfMissing";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!configuration.GetValue(ValidateEnabledConfigKey, true))
        {
            logger.LogDebug(
                "Skipping sub-ledger COA configuration validation ({ConfigKey}=false)",
                ValidateEnabledConfigKey);
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var validator = scope.ServiceProvider.GetRequiredService<ISubLedgerCoaConfigurationValidator>();

        await TryImportSeedIfMissingAsync(scope.ServiceProvider, validator, cancellationToken);

        logger.LogInformation("Validating sub-ledger base chart-of-account configuration");
        await validator.ValidateAsync(cancellationToken);
        logger.LogInformation("Sub-ledger base chart-of-account configuration is valid");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task TryImportSeedIfMissingAsync(
        IServiceProvider services,
        ISubLedgerCoaConfigurationValidator validator,
        CancellationToken cancellationToken)
    {
        if (await IsConfigurationValidAsync(validator, cancellationToken))
            return;

        var importIfMissing = configuration.GetValue(ImportSeedConfigKey, hostEnvironment.IsDevelopment());
        if (!importIfMissing)
        {
            logger.LogWarning(
                "Sub-ledger COA configuration is incomplete. Import seed data (GET /api/DbWork/import/account) " +
                "or enable {ConfigKey}=true",
                ImportSeedConfigKey);
            return;
        }

        var importer = services.GetService<ISubLedgerCoaSeedImporter>();
        if (importer is null)
        {
            logger.LogWarning(
                "Sub-ledger COA configuration is incomplete and no {Importer} is registered. " +
                "Import seed data manually via GET /api/DbWork/import/account",
                nameof(ISubLedgerCoaSeedImporter));
            return;
        }

        logger.LogWarning(
            "Sub-ledger COA configuration is incomplete; importing account seed data ({ConfigKey}=true)",
            ImportSeedConfigKey);

        await importer.ImportAccountSeedAsync(cancellationToken);

        if (!await IsConfigurationValidAsync(validator, cancellationToken))
        {
            throw new InvalidOperationException(
                $"{ErrorMessages.SubLedgerBaseAccountNotConfigured}: account seed import completed but " +
                "sub-ledger base chart-of-account mappings are still incomplete. " +
                "Ensure ERP.Host/seeding/account/SubLedgerBaseAccounts.json is present.");
        }
    }

    private static async Task<bool> IsConfigurationValidAsync(
        ISubLedgerCoaConfigurationValidator validator,
        CancellationToken cancellationToken)
    {
        try
        {
            await validator.ValidateAsync(cancellationToken);
            return true;
        }
        catch (InvalidOperationException ex) when (
            ex.Message.StartsWith(ErrorMessages.SubLedgerBaseAccountNotConfigured, StringComparison.Ordinal)
            || ex.Message.StartsWith(ErrorMessages.SubLedgerBaseAccountAmbiguous, StringComparison.Ordinal))
        {
            return false;
        }
    }
}
