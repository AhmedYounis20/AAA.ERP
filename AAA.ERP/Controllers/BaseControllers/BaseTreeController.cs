using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Resources;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseTreeController<TEntity, TInput> : BaseController<TEntity, TInput> where TEntity : BaseTreeEntity<TEntity> where TInput : BaseTreeInputModel

{
    //[HttpGet]
    //public 
    private readonly IBaseTreeService<TEntity> _service;
    public BaseTreeController(IBaseTreeService<TEntity> service, BaseInputValidator<TInput> validator,IStringLocalizer<Resource> localizaer, IMapper mapper) : base(service, validator, localizaer, mapper)
    => _service = service;
}