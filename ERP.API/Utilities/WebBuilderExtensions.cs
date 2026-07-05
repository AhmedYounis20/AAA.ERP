using System.Globalization;
using System.Text.Json.Serialization;
using Accounting.Application.Validators.Taxes;
using Accounting.Infrastructure;
using Inventory.Application.Validators.Colors;
using Inventory.Infrastructure;
using Domain.Account.DBConfiguration.DbContext;
using Identity.Infrastructure;
using ERP.Application.Data;
using ERP.Infrastracture.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
            config.RegisterServicesFromAssembly(typeof(InfrasctructureExtensions).Assembly);
            config.RegisterServicesFromAssembly(typeof(AccountingModuleExtensions).Assembly);
            config.RegisterServicesFromAssembly(typeof(InventoryModuleExtensions).Assembly);
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
            u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

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
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWt Authorization header using the Bearer scheme. \r\n\r\n" +
                               "Enter 'Bearer' [space] and you token in the text input below \r\n\r\n",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme{

                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Scheme = "oauth2",
                Name="Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
        });
    }
    public static void ConfigureApplication(this WebApplicationBuilder builder)
    {
        BuildApplication(builder).Run();
    }

    public static WebApplication BuildApplication(this WebApplicationBuilder builder)
    {
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
        });
        builder.Services.AddInfrastructureServices();
        builder.Services.AddAccountingModule();
        builder.Services.AddInventoryModule();
        builder.Services.AddPlatformModules();
        builder.Services.AddIdentityModule();
        builder.Services.AddProjectUtilities();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddAuthenticationConfiguration(builder.Configuration);
        builder.Services.AddCors();
        var app = builder.Build();

        app.UseExceptionHandler(options => { });

        app.UseSwagger();

        app.UseSwaggerUI(e =>
        {
            e.SwaggerEndpoint("/swagger/v1/swagger.json", "AAA ERP Api V1");
            e.RoutePrefix = string.Empty;
            e.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

        });

        app.UseHttpsRedirection();

        var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        if (localizeOptions != null)
            app.UseRequestLocalization(localizeOptions.Value);
        app.UseCors(e => e.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseMiddleware<LocalizationResponseMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}