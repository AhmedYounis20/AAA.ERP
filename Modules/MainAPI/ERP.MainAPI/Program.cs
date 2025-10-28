using Account.Infrastructure.ModuleRegistration;
using Identity.Infrastructure.ModuleRegistration;
using Inventory.Infrastructure.ModuleRegistration;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Shared.Application.ModuleRegistration;
using System.Reflection;
using ERP.MainAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container and load module API controllers via application parts
builder.Services
    .AddControllers()
    .AddApplicationPart(Assembly.Load("Account.API"))
    .AddApplicationPart(Assembly.Load("Inventory.API"))
    .AddApplicationPart(Assembly.Load("Identity.API"));

// Register modules
var modules = new List<IModuleRegistration>
{
    new AccountModuleRegistration(),
    new InventoryModuleRegistration(),
    new IdentityModuleRegistration()
};

foreach (var module in modules)
{
    module.RegisterDbContext(builder.Services, builder.Configuration);
    module.RegisterServices(builder.Services, builder.Configuration);
    module.RegisterMediatR(builder.Services);
    module.RegisterControllers(builder.Services);
}

// Configure application using WebBuilderExtensions
builder.ConfigureApplication();