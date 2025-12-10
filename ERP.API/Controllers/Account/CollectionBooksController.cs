using ERP.Domain.Commands.Account.CollectionBooks;
using ERP.Domain.Models.Entities.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.CollectionBooks;
using Shared.DTOs;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class CollectionBooksController : BaseSettingController<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>
{
    IBaseQueryService<CollectionBook, LookupDto> _baseQueryService;

    public CollectionBooksController(ICollectionBookService service, IBaseQueryService<CollectionBook, LookupDto> baseQueryService,
        ISender mapper) : base(service, mapper)
    => _baseQueryService = baseQueryService;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CollectionBookCreateCommand input)
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

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CollectionBookUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
