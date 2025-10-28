using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;


namespace ERP.API.Controllers.BaseControllers;

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