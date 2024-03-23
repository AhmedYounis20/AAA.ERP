using AAA.ERP.InputModels;
using AAA.ERP.InputModels.FinancialPeriods;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Resources;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AAA.ERP.Validators.InputValidators.FinancialPeriods;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinancialPeriodsController : BaseController<FinancialPeriod, FinancialPeriodInputModel>
{
    FinancialPeriodInputValidator _validator;
    FinancialPeriodUpdateValidator _updateValidator;

    IMapper _mapper;
    IStringLocalizer<Resource> _localizer;
    IFinancialPeriodService _service;

    public FinancialPeriodsController(IFinancialPeriodService service,
        FinancialPeriodInputValidator validator,
        IStringLocalizer<Resource> localizer,
        IMapper mapper,
        FinancialPeriodUpdateValidator updateValidator) : base(service, validator, localizer, mapper)
    {
        _validator = validator;
        _mapper = mapper;
        _localizer = localizer;
        _service = service;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] FinancialPeriodInputModel input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpGet("GetCurrentFinancialPeriod")]
    public virtual async Task<IActionResult> GetCurrentFinancialPeriod()
    {
        var result = await _service.GetCurrentFinancailPeriod();
        return StatusCode((int)result.StatusCode,result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] FinancialPeriodUpdateInputModel input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    protected virtual async Task<IActionResult> UpdateRecord(Guid id, FinancialPeriodUpdateInputModel input)
    {
        var validationResult = _updateValidator.Validate(input);
        if (validationResult.IsValid)
        {
            var entity = _mapper.Map<FinancialPeriod>(input);
            entity.Id = id;
            var userId = User.Claims.FirstOrDefault(e => e.Type == "id").Value;
            entity.ModifiedBy = Guid.Parse(userId);
            var result = await _service.Update(entity);
            return StatusCode((int)result.StatusCode, result);
        }
        else
        {
            return BadRequest(
               new ApiResponse
               {
                   IsSuccess = false,
                   StatusCode = HttpStatusCode.BadRequest,
                   ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
               });
        }
    }
}