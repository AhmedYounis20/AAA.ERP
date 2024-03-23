using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Models.Entities.Identity;
using AAA.ERP.Utility;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Impelementation;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AAA.ERP.Validators.InputValidators.FinancialPeriods;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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


builder.Services.AddControllers();
//.AddJsonOptions(options =>
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddServices();
builder.Services.AddScoped<AccountGuideValidator>();
builder.Services.AddScoped<CurrencyValidator>();
builder.Services.AddScoped<GLSettingValidator>();
builder.Services.AddScoped<FinancialPeriodInputValidator>();
builder.Services.AddScoped<FinancialPeriodUpdateValidator>();

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
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

builder.Services.AddScoped(typeof(IBaseBussinessValidator<>), typeof(BaseBussinessValidator<>));
builder.Services.AddScoped(typeof(IBaseSettingBussinessValidator<>), typeof(BaseSettingBussinessValidator<>));
builder.Services.AddScoped<ICurrencyBussinessValidator, CurrencyBussinessValidator>();
builder.Services.AddScoped<IBaseSettingBussinessValidator<Currency>, CurrencyBussinessValidator>();
builder.Services.AddScoped<IBaseSettingBussinessValidator<AccountGuide>, AccountGuideBussinessValidator>();

builder.Services.AddScoped<IBaseBussinessValidator<FinancialPeriod>, FinancialPeriodBussinessValidator>();

builder.Services.AddScoped<IAccountGuideBussinessValidator, AccountGuideBussinessValidator>();
builder.Services.AddScoped<IFinancialPeriodBussinessValidator, FinancialPeriodBussinessValidator>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
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
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(u =>
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
