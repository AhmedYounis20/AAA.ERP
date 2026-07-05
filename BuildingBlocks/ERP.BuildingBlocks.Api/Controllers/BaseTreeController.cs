using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERP.BuildingBlocks.Api.Controllers;

/// <summary>
/// CRUD base for hierarchical tree entities.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseTreeController<TEntity, TCreate, TUpdate>
    : BaseController<TEntity, TCreate, TUpdate>
    where TEntity : BaseTreeEntity<TEntity>
    where TCreate : BaseTreeCreateCommand<TEntity>
    where TUpdate : BaseTreeUpdateCommand<TEntity>
{
    private readonly IBaseTreeService<TEntity, TCreate, TUpdate> _service;

    public BaseTreeController(IBaseTreeService<TEntity, TCreate, TUpdate> service,
        ISender sender) : base(service, sender)
        => _service = service;
}
