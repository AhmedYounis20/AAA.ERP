using AAA.ERP.Controllers.BaseControllers;
using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Commands.SubLeadgers.Banks;
using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Commands.SubLeadgers.Customers;
using Domain.Account.Commands.SubLeadgers.Suppliers;
using Domain.Account.InputModels.Subleadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Services.Interfaces.SubLeadgers;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers.SubLeadgers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : BaseTreeSettingController<Supplier,SupplierCreateCommand, SupplierUpdateCommand>
{
    private ISupplierService _service;
    private readonly IStringLocalizer<Resource> _localizer;
    private ISender _sender;

    public SuppliersController(IStringLocalizer<Resource> localizer, ISupplierService service, ISender sender)
        : base(service, localizer, sender)
    {
        _localizer = localizer;
        _service = service;
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRecord(SupplierCreateCommand input)
    {
        var result = await _sender.Send(input);
        result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRecords()
    {
        var result = await _service.ReadAll();
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecord(Guid id)
    {
        var result = await _service.ReadById(id);
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(Guid id, SupplierUpdateCommand input)
    {
        var result = await _service.Update(input);
        return StatusCode((int) result.StatusCode, result);
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