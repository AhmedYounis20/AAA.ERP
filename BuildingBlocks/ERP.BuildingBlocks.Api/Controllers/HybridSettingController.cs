using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// Hybrid CQRS base for standard setting entities: writes via <see cref="ISender"/>, reads via <see cref="ISettingQueries{TEntity}"/>.
/// </summary>
/// <remarks>ADR-012 compliant — does not inject <c>I*Service</c> write services.</remarks>
public abstract class HybridSettingController<TEntity, TCreate, TUpdate>(
    ISender sender,
    ISettingQueries<TEntity> queries) : ControllerBase
    where TEntity : BaseSettingEntity
    where TCreate : BaseSettingCreateCommand<TEntity>
    where TUpdate : BaseSettingUpdateCommand<TEntity>
{
    protected ISender Sender { get; } = sender;
    protected ISettingQueries<TEntity> Queries { get; } = queries;

    protected abstract IRequest<ApiResponse<TEntity>> BuildDeleteCommand(Guid id);

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TCreate input)
        => this.ToResult(await Sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public virtual async Task<IActionResult> Get()
        => this.ToResult(await Queries.GetAll());

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated(
        [FromQuery] SettingFilterDto filter,
        CancellationToken cancellationToken)
        => this.ToResult(await Queries.GetPaginated(filter, cancellationToken));

    [HttpGet("search")]
    public virtual async Task<IActionResult> Search([FromQuery] string query)
        => this.ToResult(await Queries.SearchByName(query));

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
        => this.ToResult(await Queries.GetById(id));

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdate input)
    {
        input.Id = id;
        return this.ToResult(await Sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await Sender.Send(BuildDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
