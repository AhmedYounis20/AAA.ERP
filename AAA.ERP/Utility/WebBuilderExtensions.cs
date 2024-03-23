﻿using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Repositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Services.Impelementation;
using AAA.ERP.Services.Interfaces;

namespace AAA.ERP.Utility;

public static class WebBuilderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(IBaseSettingRepository<>), typeof(BaseSettingRepository<>));
        services.AddScoped(typeof(IBaseSettingService<>), typeof(BaseSettingService<>));
        services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

        services.AddScoped<IAccountGuideRepository, AccountGuideRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IGLSettingRepository, GLSettingRepository>();
        services.AddScoped<IFinancialPeriodRepository, FinancialPeriodRepository>();

        services.AddScoped<IAccountGuideService, AccountGuideService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IGLSettingService, GLSettingService>();
        services.AddScoped<IFinancialPeriodService, FinancialPeriodService>();

    }
}