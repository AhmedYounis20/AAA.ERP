using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Repositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Services.Impelementation;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Impelementation;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Validators.InputValidators.FinancialPeriods;
using AAA.ERP.Validators.InputValidators;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.Identity;
using Microsoft.Extensions.Options;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;

namespace AAA.ERP.Utility;

public static class WebBuilderExtensions
{
    public static void AddProjectUtilities(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddEndpointsApiExplorer();


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
    public static void AddAuthenticationConfiguration(this IServiceCollection services,IConfiguration configuration)
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
            };
        });
    }
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWt Authorization header using the Bearer scheme. \r\n\r\n" +
                               "Enter 'Bearer' [space] and you token in the text input below \r\n\r\n",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
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
        services.AddScoped(typeof(IBaseSettingService<>), typeof(BaseSettingService<>));
        services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddScoped<ExportDataToSeed>();
        services.AddScoped<ImportDataToSeed>();

        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();
    }
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseBussinessValidator<>), typeof(BaseBussinessValidator<>));
        services.AddScoped(typeof(IBaseSettingBussinessValidator<>), typeof(BaseSettingBussinessValidator<>));
        services.AddScoped<ICurrencyBussinessValidator, CurrencyBussinessValidator>();
        services.AddScoped<IBaseSettingBussinessValidator<Currency>, CurrencyBussinessValidator>();
        services.AddScoped<IBaseSettingBussinessValidator<AccountGuide>, AccountGuideBussinessValidator>();
        services.AddScoped<IBaseBussinessValidator<FinancialPeriod>, FinancialPeriodBussinessValidator>();
        services.AddScoped<IAccountGuideBussinessValidator, AccountGuideBussinessValidator>();
        services.AddScoped<IAccountGuideBussinessValidator, AccountGuideBussinessValidator>();
        services.AddScoped<IFinancialPeriodBussinessValidator, FinancialPeriodBussinessValidator>();
        services.AddScoped<IChartOfAccountBussinessValidator, ChartOfAccountBussinessValidator>();

        // fluent validators
        services.AddScoped<AccountGuideInputValidator>();
        services.AddScoped<CurrencyInputValidator>();
        services.AddScoped<GLSettingInputValidator>();
        services.AddScoped<FinancialPeriodInputValidator>();
        services.AddScoped<FinancialPeriodUpdateValidator>();
        services.AddScoped<ChartOfAccountInputValidator>();

    }
    public static void AddReositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();
        services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
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
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbConnection"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });


        //.AddJsonOptions(options =>
        //        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        builder.Services.AddControllers();
        builder.Services.AddServices();
        builder.Services.AddReositories();
        builder.Services.AddValidators();
        builder.Services.AddProjectUtilities();
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