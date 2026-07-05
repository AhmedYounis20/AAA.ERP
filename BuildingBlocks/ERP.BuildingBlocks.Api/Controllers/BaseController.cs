using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// Generic CRUD controller base for entities backed by <see cref="IBaseService{TEntity,TCreate,TUpdate}"/>.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BaseController<TEntity, TCreate, TUpdate> : ControllerBase
    where TEntity : BaseEntity
    where TCreate : BaseCreateCommand<TEntity>
    where TUpdate : BaseUpdateCommand<TEntity>
{
    private readonly IBaseService<TEntity, TCreate, TUpdate> _service;
    private readonly ISender _sender;

    /// <summary>Resolved from <c>Accept-Language</c> or <c>Accept-Culture</c> headers (<c>ar</c> vs <c>en</c>).</summary>
    public string CurrentLanguage => HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
        HttpContext.Request.Headers["Accept-Language"].Any(e => e.Contains("ar")) ||
        HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
        HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar")) ? "ar" : "en";

    public BaseController(IBaseService<TEntity, TCreate, TUpdate> service,
        ISender sender)
    {
        _service = service;
        _sender = sender;
    }

    /// <summary>Creates a record via MediatR and returns the standard API envelope.</summary>
    protected virtual async Task<IActionResult> CreateRecord(TCreate input)
    {
        var result = await _sender.Send(input);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.CreatedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Reads all records without pagination.</summary>
    protected virtual async Task<IActionResult> GetAllRecords()
    {
        var result = await _service.ReadAll();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Gets paginated records with filtering support.</summary>
    protected virtual async Task<IActionResult> GetAllRecordsPaginated(BaseFilterDto filter, CancellationToken cancellationToken = default)
    {
        var result = await _service.ReadAllPaginated(filter, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Reads a single record by identifier.</summary>
    protected virtual async Task<IActionResult> GetRecord(Guid id)
    {
        var result = await _service.ReadById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates a record via MediatR and returns the standard API envelope.</summary>
    protected virtual async Task<IActionResult> UpdateRecord(Guid id, TUpdate input)
    {
        input.Id = id;
        var result = await _sender.Send(input);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.UpdatedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Deletes a record by identifier.</summary>
    protected virtual async Task<IActionResult> DeleteRecord(Guid id)
    {
        var result = await _service.Delete(id);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.DeletedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Bulk creates multiple records.</summary>
    protected virtual async Task<IActionResult> CreateBulkRecords(List<TEntity> entities)
    {
        var result = await _service.Create(entities);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.BulkCreatedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Bulk updates multiple records.</summary>
    protected virtual async Task<IActionResult> UpdateBulkRecords(List<TEntity> entities)
    {
        var result = await _service.Update(entities);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.BulkUpdatedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Bulk deletes multiple records.</summary>
    protected virtual async Task<IActionResult> DeleteBulkRecords(List<TEntity> entities)
    {
        var result = await _service.Delete(entities);
        if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = SuccessMessages.BulkDeletedSuccessfully };
        return StatusCode((int)result.StatusCode, result);
    }
}
