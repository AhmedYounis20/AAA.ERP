using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ERP.Host.Utilities;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            foreach (var (documentName, displayName) in SwaggerModuleGroups.Documents)
            {
                options.SwaggerDoc(documentName, new OpenApiInfo
                {
                    Title = documentName == SwaggerModuleGroups.All
                        ? "AAA ERP API"
                        : $"AAA ERP — {displayName}",
                    Version = "v1"
                });
            }

            IncludeXmlComments(options, Assembly.GetExecutingAssembly());
            IncludeXmlComments(options, typeof(Identity.Application.IAuthenticationService).Assembly);

            options.DocInclusionPredicate((documentName, apiDescription) =>
            {
                if (documentName == SwaggerModuleGroups.All)
                    return true;

                var group = SwaggerModuleGroupResolver.FromApiDescription(apiDescription);
                return string.Equals(documentName, group, StringComparison.OrdinalIgnoreCase);
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                    "Enter 'Bearer' [space] and your token in the text input below \r\n\r\n",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    public static WebApplication UseModuleSwaggerUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var (documentName, displayName) in SwaggerModuleGroups.Documents)
            {
                options.SwaggerEndpoint($"/swagger/{documentName}/swagger.json", displayName);
            }

            options.RoutePrefix = string.Empty;
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        });

        return app;
    }

    private static void IncludeXmlComments(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options, Assembly assembly)
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
}
