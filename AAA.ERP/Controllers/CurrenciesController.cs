using AAA.ERP.InputModels;
using AAA.ERP.Models.Data.Currencies;
using AAA.ERP.Resources;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyInputModel>
{
    public CurrenciesController(IBaseSettingService<Currency> service, CurrencyValidator validator, IStringLocalizer<Resource> localizer, IMapper mapper) : base(service, validator, localizer, mapper)
    { }
}