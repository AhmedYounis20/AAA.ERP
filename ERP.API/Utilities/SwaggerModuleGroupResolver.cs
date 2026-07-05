using ERP.BuildingBlocks.Application.Logging;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ERP.Api.Utilities;

public static class SwaggerModuleGroupResolver
{
    public static string? FromControllerType(Type? controllerType)
    {
        if (controllerType is null)
            return null;

        return ModuleNameResolver.FromType(controllerType)?.ToLowerInvariant();
    }

    public static string? FromApiDescription(Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription apiDescription)
    {
        if (apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerAction)
            return null;

        return FromControllerType(controllerAction.ControllerTypeInfo.AsType());
    }
}
