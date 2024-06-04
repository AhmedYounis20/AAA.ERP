using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.InputModels.BaseInputModels;
using Domain.Account.Services.BaseServices.interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.BaseEntities;
using Shared.Resources;
using Shared.Responses;

namespace AAA.ERP.Controllers.BaseControllers;

[Route("api/[controller]")]
[ApiController]
public class BaseTreeController<TEntity, TCreate,TUpdate> 
    : BaseController<TEntity, TCreate,TUpdate> 
    where TEntity : BaseTreeEntity<TEntity> 
    where TCreate : BaseTreeCreateCommand<ApiResponse<TEntity>>
    where TUpdate : BaseTreeUpdateCommand<ApiResponse<TEntity>>
{
    //[HttpGet]
    //public 
    private readonly IBaseTreeService<TEntity> _service;
    public BaseTreeController(IBaseTreeService<TEntity> service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    => _service = service;
}