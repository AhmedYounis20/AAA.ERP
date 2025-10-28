using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;

namespace ERP.MainAPI.Utilities;

public static class WebBuilderExtensions
{
    public static void AddProjectUtilities(this IServiceCollection services)
    {
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key ?? "")),
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

    public static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
    }

    public static void AddJsonConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    public static void ConfigureApplication(this WebApplicationBuilder builder)
    {
        // Configure Identity options
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });

        // Add HTTP context accessor
        builder.Services.AddHttpContextAccessor();

        // Add project utilities
        builder.Services.AddProjectUtilities();
        
        // Add JSON configuration
        builder.Services.AddJsonConfiguration();
        
        // Add Swagger configuration
        builder.Services.AddSwaggerConfiguration();
        
        // Add authentication configuration
        builder.Services.AddAuthenticationConfiguration(builder.Configuration);
        
        // Add CORS configuration
        builder.Services.AddCorsConfiguration();

        var app = builder.Build();

        // Configure the HTTP request pipeline
        app.UseSwagger();
        app.UseSwaggerUI(e =>
        {
            e.SwaggerEndpoint("/swagger/v1/swagger.json", "AAA ERP Api V1");
            e.RoutePrefix = string.Empty;
            e.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });

        app.UseHttpsRedirection();

        // Configure localization
        var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
        if (localizeOptions != null)
            app.UseRequestLocalization(localizeOptions.Value);

        // Configure CORS
        app.UseCors();

        // Configure authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
