using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for manufacturer company settings.</summary>
[Route("api/[controller]")]
[ApiController]
public class ManufacturerCompaniesController : BaseSettingController<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>
{
    public ManufacturerCompaniesController(IManufacturerCompanyService service,
        ISender mapper) : base(service, mapper)
    { }

    /// <summary>Creates a manufacturer company.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ManufacturerCompanyCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all manufacturer companies.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists manufacturer companies with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a manufacturer company by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Updates a manufacturer company.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ManufacturerCompanyUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a manufacturer company.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
