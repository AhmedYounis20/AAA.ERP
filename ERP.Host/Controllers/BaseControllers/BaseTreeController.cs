using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;


namespace ERP.Host.Controllers.BaseControllers;

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
    //[HttpGet]
    //public 
    private readonly IBaseTreeService<TEntity, TCreate, TUpdate> _service;
    public BaseTreeController(IBaseTreeService<TEntity, TCreate, TUpdate> service,
        ISender sender) : base(service, sender)
    => _service = service;
}
