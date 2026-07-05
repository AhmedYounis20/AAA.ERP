using System.Text.Json;
using ERP.BuildingBlocks.Application.Logging;
using FluentAssertions;

namespace ERP.UnitTests.Logging;

/// <summary>
/// Phase 4.1.1 — production logging configuration regression checks.
/// </summary>
public class ProductionLoggingConfigurationTests
{
    private static string ReadApiConfig(string fileName)
    {
        var path = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "ERP.API",
            fileName));

        return File.ReadAllText(path);
    }

    [Fact]
    public void Production_appsettings_uses_compact_json_console_sink()
    {
        var json = ReadApiConfig("appsettings.Production.json");
        using var doc = JsonDocument.Parse(json);

        var writeTo = doc.RootElement.GetProperty("Serilog").GetProperty("WriteTo");
        writeTo.GetArrayLength().Should().BeGreaterThan(0);

        var consoleSink = writeTo.EnumerateArray().First(e =>
            e.GetProperty("Name").GetString() == "Console");

        var formatter = consoleSink.GetProperty("Args").GetProperty("formatter").GetString();
        formatter.Should().Contain("CompactJsonFormatter");
    }

    [Fact]
    public void Base_appsettings_uses_compact_json_for_non_development_environments()
    {
        var json = ReadApiConfig("appsettings.json");
        json.Should().Contain("CompactJsonFormatter");
    }

    [Fact]
    public void Development_appsettings_includes_CorrelationId_in_output_template()
    {
        var json = ReadApiConfig("appsettings.Development.json");
        json.Should().Contain(LoggingContextPropertyNames.CorrelationId);
    }

    [Fact]
    public void Api_source_must_not_call_Log_CloseAndFlush()
    {
        var apiRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "ERP.API"));
        var csFiles = Directory.EnumerateFiles(apiRoot, "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                     && !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"));

        var offenders = csFiles
            .Where(path => File.ReadAllText(path).Contains("Log.CloseAndFlush(", StringComparison.Ordinal))
            .ToList();

        offenders.Should().BeEmpty("explicit Serilog flush breaks WebApplicationFactory; use Host.UseSerilog instead");
    }
}
