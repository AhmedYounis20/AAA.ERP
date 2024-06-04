using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.InputModels.Subleadgers;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers.SubLeadgers;
[Route("api/[controller]")]
[ApiController]
public class CashInBoxController : ControllerBase
{
    private ICashInBoxService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;
    public CashInBoxController(IStringLocalizer<Resource> localizer, ICashInBoxService service,ISender sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }

    // [HttpGet("GetLevel")]
    // public virtual async Task<IActionResult> GetLevel([FromQuery] int level = 0)
    //     => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetLevel(level), StatusCode = HttpStatusCode.OK });

    // [HttpGet("GetChildren/{parentId}")]
    // public virtual async Task<IActionResult> GetChildren(Guid parentId, [FromQuery] int level = 0)
    //     => Ok(new ApiResponse { IsSuccess = true, Result = await _service.GetChildren(parentId,level), StatusCode = HttpStatusCode.OK });

    [HttpPost]
    public async Task<IActionResult> CreateRecord(BaseSubLeadgerInputModel input)
    {
        // var validationResult = _validator.Validate(input);
        // if (validationResult.IsValid)
        // {
            // var userId = User.Claims.FirstOrDefault(e => e.Type == "id")?.Value;
            // var entity = _mapper.Map<TEntity>(input);
            // entity.CreatedBy = Guid.Parse(userId);
            var result = await _sender.Send(input);
            result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
            return StatusCode((int) result.StatusCode, result);
        // }
        // else
        // {
        //     return BadRequest(
        //         new ApiResponse
        //         {
        //             IsSuccess = false,
        //             StatusCode = HttpStatusCode.BadRequest,
        //             ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
        //         });
        // }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRecords()
    {
        var result = await _service.Get();
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecord(Guid id)
    {
        var result = await _service.Get(id);
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(Guid id, BaseSubLeadgerInputModel input)
    {
        // var validationResult = _validator.Validate(input);
        // if (validationResult.IsValid)
        // {
        //     var entity = _mapper.Map<TEntity>(input);
            input.Id = id;
            // var userId = User.Claims.FirstOrDefault(e => e.Type == "id").Value;
            // entity.ModifiedBy = Guid.Parse(userId);
            var result = await _service.Update(input);
            return StatusCode((int) result.StatusCode, result);
        // }
        // else
        // {
        //     return BadRequest(
        //         new ApiResponse
        //         {
        //             IsSuccess = false,
        //             StatusCode = HttpStatusCode.BadRequest,
        //             ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
        //         });
        // }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(Guid id)
    {
        var result = await _service.Delete(id);
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();

        return StatusCode((int) result.StatusCode, result);
    }
    
    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GetNextSubLeadgers(parentId));
    }
}