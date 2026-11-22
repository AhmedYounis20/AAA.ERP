using Accounting.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Accounting.Infrastructure.Validation;

/// <summary>
/// Optionally validates sub-ledger base COA configuration at startup.
/// Disabled by default — seed data is loaded via GET /api/DbWork/import/account, not on host start.
/// </summary>
public sealed class SubLedgerCoaConfigurationStartupValidator(
    IServiceScopeFactory serviceScopeFactory,
    IConfiguration configuration,
    ILogger<SubLedgerCoaConfigurationStartupValidator> logger) : IHostedService
{
    private const string ValidateEnabledConfigKey = "Accounting:SubLedger:ValidateConfigurationOnStartup";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!configuration.GetValue(ValidateEnabledConfigKey, false))
        {
            logger.LogDebug(
                "Skipping sub-ledger COA configuration validation ({ConfigKey}=false)",
                ValidateEnabledConfigKey);
            return;
        }

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var validator = scope.ServiceProvider.GetRequiredService<ISubLedgerCoaConfigurationValidator>();

        logger.LogInformation("Validating sub-ledger base chart-of-account configuration");
        await validator.ValidateAsync(cancellationToken);
        logger.LogInformation("Sub-ledger base chart-of-account configuration is valid");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
