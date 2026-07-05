# Production logging runbook (Phase 4.1.1)

Structured logging for **AAA.ERP** uses [Serilog](https://serilog.net/) with request-scoped enrichers. This document covers production sinks, required log fields, and constraints for container / test hosts.

## Architecture

| Component | Location |
|-----------|----------|
| Host wiring | `ERP.API/Utilities/SerilogExtensions.cs` — `AddSerilogLogging()` |
| Enrichers | `BuildingBlocks/ERP.BuildingBlocks.Application/Logging/Enrichers/` |
| Correlation middleware | `CorrelationIdMiddleware` — header `X-Correlation-Id` |
| Module middleware | `RequestModuleMiddleware` — Swagger module group per request |
| Command pipeline | `LoggingBehavior<,>` — sets `CommandName` + `Module` on MediatR writes |

### Standard properties on every HTTP request

| Property | Source |
|----------|--------|
| `CorrelationId` | `CorrelationIdMiddleware` + `CorrelationIdEnricher` |
| `Module` | `RequestModuleMiddleware` / `ModuleEnricher` |
| `CommandName` | `LoggingBehavior` during MediatR commands |
| `Application` | Serilog config `Properties` (`AAA.ERP`) |

Clients may send `X-Correlation-Id`; the API echoes it on the response. If omitted, a new GUID is generated.

## Configuration by environment

| Environment | Config files | Console format |
|-------------|--------------|----------------|
| **Production** | `appsettings.json` + `appsettings.Production.json` | **Compact JSON** → stdout |
| **Development** | + `appsettings.Development.json` | Human-readable template with `{CorrelationId}` |
| **Integration / smoke tests** | `Development` (factory override) | Human-readable (see test factories) |

Set the environment:

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet ERP.API.dll
```

### Example production log line (JSON)

```json
{
  "@t": "2026-07-05T14:30:00.1234567Z",
  "@l": "Information",
  "@mt": "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms",
  "CorrelationId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "Module": "Inventory",
  "CommandName": "InventoryTransactionCreateCommand",
  "Application": "AAA.ERP",
  "Environment": "Production"
}
```

`CorrelationId`, `Module`, and `CommandName` appear as **top-level JSON properties** when present (via enrichers + `LogContext`).

## Sink options

### Default: structured JSON to stdout (containers)

Production uses `CompactJsonFormatter` writing to **Console** (stdout). This is the recommended path for:

- Docker / Kubernetes (collect via cluster logging agent)
- Azure App Service log stream
- IIS stdout when containerized

No extra packages required (`Serilog.Formatting.Compact` is already referenced).

**Verify locally:**

```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run --project ERP.API/ERP.API.csproj 2>&1 | head -5
```

Each line should be a single JSON object. Issue a request with `X-Correlation-Id: test-correlation-001` and confirm the value appears in subsequent log lines.

### Optional: Seq

For a dedicated log UI, add the Seq sink package and extend configuration.

1. Add package to `ERP.API.csproj`:

   ```xml
   <PackageReference Include="Serilog.Sinks.Seq" Version="8.0.0" />
   ```

2. Add a second `WriteTo` entry (or replace via environment variables):

   ```json
   {
     "Name": "Seq",
     "Args": {
       "serverUrl": "https://seq.example.com",
       "apiKey": ""
     }
   }
   ```

3. Prefer **secrets / env vars** for URL and API key (do not commit keys):

   ```bash
   export Serilog__WriteTo__1__Name=Seq
   export Serilog__WriteTo__1__Args__serverUrl=https://seq.example.com
   export Serilog__WriteTo__1__Args__apiKey=<from-key-vault>
   ```

Keep the **Console** sink enabled in production when running in containers so platform log collection continues to work.

## WebApplicationFactory / integration tests

**Do not** add `Log.CloseAndFlush()` in `Program.cs` or after `WebApplicationFactory` teardown.

- Serilog is registered via `builder.Host.UseSerilog(..., preserveStaticLogger: true)` in `SerilogExtensions`.
- The generic host flushes Serilog on shutdown; an explicit `CloseAndFlush()` disposes the static logger and breaks subsequent factory instances in the same test process.

Smoke and integration factories set `UseEnvironment("Development")` for readable console output during tests.

## Troubleshooting

| Symptom | Check |
|---------|--------|
| Missing `CorrelationId` | Middleware order in `WebBuilderExtensions.BuildApplication` — `CorrelationIdMiddleware` must run before controllers |
| Missing `Module` | `RequestModuleMiddleware` after routing; controller namespace must match `SwaggerModuleGroups` |
| Missing `CommandName` | Request must go through MediatR `LoggingBehavior` (writes only) |
| Plain text in Production | `ASPNETCORE_ENVIRONMENT` must be `Production`; Development overlay overrides formatter |
| Tests fail after logging change | Search repo for `Log.CloseAndFlush` — must be zero matches |

## Related

- [MODULAR_MONOLITH.md §10.6](../architecture/MODULAR_MONOLITH.md#106-phase-4--hardening) — Phase 4.1.1 deliverable
- `Tests/ERP.UnitTests/Logging/ProductionLoggingConfigurationTests.cs` — config regression checks
