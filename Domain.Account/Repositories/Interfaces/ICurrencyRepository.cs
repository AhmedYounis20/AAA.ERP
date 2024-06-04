﻿using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.Interfaces;

public interface ICurrencyRepository: IBaseSettingRepository<Currency> {

    public Task<Currency?> GetDefaultCurrency();
    public Task<bool> IsExitedCurrencySymbol(string? symbol);
}
