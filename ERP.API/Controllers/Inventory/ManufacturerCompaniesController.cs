using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using Shared.DTOs;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerCompaniesController : BaseSettingController<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>
{
    public ManufacturerCompaniesController(IManufacturerCompanyService service,
        ISender mapper) : base(service, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ManufacturerCompanyCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ManufacturerCompanyUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
