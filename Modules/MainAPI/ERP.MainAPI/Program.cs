using Account.Infrastructure.ModuleRegistration;
using Identity.Infrastructure.ModuleRegistration;
using Inventory.Infrastructure.ModuleRegistration;
using Shared.Application.ModuleRegistration;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();