using System.Globalization;
using Domain.Account.DBConfiguration.DbContext;
using ERP.Application.Data;
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
        builder.Services.AddInfrastructureServices();
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