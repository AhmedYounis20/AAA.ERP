using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;
using Shared.DTOs;

namespace ERP.Host.Controllers.Account;

/// <summary>CRUD API for collection book settings and lookups.</summary>
[Route("api/[controller]")]
[ApiController]
public class CollectionBooksController : BaseSettingController<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>
{
    IBaseQueryService<CollectionBook, LookupDto> _baseQueryService;

    public CollectionBooksController(ICollectionBookService service, IBaseQueryService<CollectionBook, LookupDto> baseQueryService,
        ISender mapper) : base(service, mapper)
    => _baseQueryService = baseQueryService;

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CollectionBookCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Returns lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<LookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<LookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<LookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CollectionBookUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
