using System.Globalization;
using System.Text.Json.Serialization;
using Accounting.Application.Validators.Taxes;
using Accounting.Infrastructure;
using Inventory.Application.Validators.Colors;
using Inventory.Infrastructure;
using Domain.Account.DBConfiguration.DbContext;
using Identity.Infrastructure;
using ERP.BuildingBlocks.Persistence.Data;
using ERP.BuildingBlocks.Application.Extensions;
using ERP.BuildingBlocks.Application.Logging.Middleware;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastructure.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Platform.Infrastructure;
using Platform.Localization;
using Shared.BaseEntities.Identity;
using Shared.Behaviors;
using Shared.Exceptions.Handler;

namespace ERP.Api.Utilities;

public static class WebBuilderExtensions
{
    public static void AddProjectUtilities(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationDbContext).Assembly);
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddEndpointsApiExplorer();
        services.AddValidatorsFromAssembly(typeof(IApplicationDbContext).Assembly);
        services.AddValidatorsFromAssembly(typeof(TaxCreateValidator).Assembly);
        services.AddValidatorsFromAssembly(typeof(ColorCreateValidator).Assembly);
        services.AddMediatR(config =>
        {
            // Single registration point for all command handlers (Phase 0 — MODULAR_MONOLITH.md §10.2).
            config.RegisterServicesFromAssembly(typeof(InfrastructureExtensions).Assembly);
            config.RegisterServicesFromAssembly(typeof(AccountingModuleExtensions).Assembly);
            config.RegisterServicesFromAssembly(typeof(InventoryModuleExtensions).Assembly);
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        // Register Global Exception Handler
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddProblemDetails();
        
        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(
            opt =>
            {
                var supportedResources = new List<CultureInfo>
                {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG")
                };

                opt.DefaultRequestCulture = new RequestCulture("en-US");
                opt.SupportedCultures = supportedResources;
                opt.SupportedUICultures = supportedResources;
            }
            );
    }
    public static void AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("ApiSettings:Secret");

        services.AddAuthentication(u =>
        {
            u.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            u.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(u =>
        {
            u.RequireHttpsMetadata = false;
            u.SaveToken = true;
            u.TokenValidationParameters = new()
            {

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };
        });
    }
    public static void ConfigureApplication(this WebApplicationBuilder builder)
    {
        BuildApplication(builder).Run();
    }

    public static WebApplication BuildApplication(this WebApplicationBuilder builder)
    {
        builder.AddSerilogLogging();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

        });
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection"), b => b.MigrationsAssembly("ERP.API"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        });
        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opts.JsonSerializerOptions.Converters.Add(new EntryTypeJsonConverter());
        });
        builder.Services.AddInfrastructureServices();
        builder.Services.AddAccountingModule();
        builder.Services.AddInventoryModule();
        builder.Services.AddPlatformModules();
        builder.Services.AddIdentityModule();
        builder.Services.AddProjectUtilities();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddErpStructuredLogging();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddAuthenticationConfiguration(builder.Configuration);
        builder.Services.AddCors();
        var app = builder.Build();

        app.UseExceptionHandler(options => { });
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseModuleSwaggerUi();

        app.UseHttpsRedirection();

        var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        if (localizeOptions != null)
            app.UseRequestLocalization(localizeOptions.Value);
        app.UseCors(e => e.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseMiddleware<LocalizationResponseMiddleware>();
        app.UseRouting();
        app.UseMiddleware<RequestModuleMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}