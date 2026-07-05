using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services.Transactions;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Transactions;

/// <summary>CRUD API for stock import transactions (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImportTransactionsController(ISender sender, IImportTransactionQueries queries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ImportTransactionCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] InventoryTransactionFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ImportTransactionUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new ImportTransactionDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("GetTransactionNumber")]
    public async Task<IActionResult> GetTransactionNumber([FromQuery] DateTime dateTime)
        => this.ToResult(await queries.GetTransactionNumber(dateTime));
}
