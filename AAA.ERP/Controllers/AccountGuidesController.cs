using AAA.ERP.InputModels;
using AAA.ERP.Models.Data.AccountGuide;
using AAA.ERP.Resources;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountGuidesController : BaseSettingController<AccountGuide, AccountGuideInputModel>
{
    public AccountGuidesController(IAccountGuideService service,
        AccountGuideValidator validator,
        IStringLocalizer<Resource> localizer,
        IMapper mapper) : base(service, validator, localizer, mapper)
    { }
}