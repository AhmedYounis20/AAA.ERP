using System.Globalization;
using AAA.ERP.Services.Impelementation;
using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Services.Identity;
using Domain.Account.Utility;
using ERP.Application.Data;
using ERP.Application.Repositories;
using ERP.Application.Repositories.BaseRepositories;
using ERP.Application.Repositories.SubLeadgers;
using ERP.Application.Services.Account.Entries;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Application.Services.Identity;
using ERP.Infrastracture.Repositories.Account;
using ERP.Infrastracture.Repositories.Account.SubLeadgers;
using ERP.Infrastracture.Repositories.BaseRepositories;
using ERP.Infrastracture.Services.Account;
using ERP.Infrastracture.Services.Account.Entries;
using ERP.Infrastracture.Services.Account.SubLeadgers;
using ERP.Infrastracture.Services.BaseServices;
using ERP.Infrastracture.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Shared.BaseEntities.Identity;
using Shared.Behaviors;

namespace ERP.Api.Utilities;

public static class WebBuilderExtensions
{
    public static void AddProjectUtilities(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationDbContext).Assembly);
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddEndpointsApiExplorer();
        services.AddValidatorsFromAssembly(typeof(IApplicationDbContext).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(ApplicationDbContext).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
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
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseSettingService<,,>), typeof(BaseSettingService<,,>));
        services.AddScoped(typeof(IBaseService<,,>), typeof(BaseService<,,>));
        services.AddScoped<ExportDataToSeed>();
        services.AddScoped<ImportDataToSeed>();

        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
        services.AddScoped<ICashInBoxService, CashInBoxService>();
        services.AddScoped<IBankService, BankService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IFixedAssetService, FixedAssetService>();
        services.AddScoped<ICostCenterService, CostCenterService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IComplexEntryService, ComplexEntryService>();
        services.AddScoped<IEntryService, EntryService>();
        services.AddScoped<IOpeningEntryService, OpeningEntryService>();
        services.AddScoped<IJournalEntryService, JournalEntryService>();
        services.AddScoped<IPaymentEntryService, PaymentEntryService>();
        services.AddScoped<IReceiptEntryService, ReceiptEntryService>();
        services.AddScoped<ICompinedEntryService, CompinedEntryService>();
        services.AddScoped<ICollectionBookService, CollectionBookService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
        services.AddScoped<ICashInBoxRepository, CashInBoxRepository>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IFixedAssetRepository, FixedAssetRepository>();
        services.AddScoped<ICostCenterRepository, CostCenterRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<ICollectionBookRepository, CollectionBookRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddHttpContextAccessor();
    }
    public static void ConfigureApplication(this WebApplicationBuilder builder)
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


        //.AddJsonOptions(options =>
        //        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddControllers();
        builder.Services.AddServices();
        builder.Services.AddRepositories();
        builder.Services.AddProjectUtilities();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddAuthenticationConfiguration(builder.Configuration);

        builder.Services.AddCors();

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI();
        }
        else
        {
            app.UseSwaggerUI(e =>
            {
                e.SwaggerEndpoint("/swagger/v1/swagger.json", "AAA ERP Api V1");
                e.RoutePrefix = string.Empty;
            });
        }
        app.UseHttpsRedirection();

        var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        if (localizeOptions != null)
            app.UseRequestLocalization(localizeOptions.Value);
        app.UseCors(e => e.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}