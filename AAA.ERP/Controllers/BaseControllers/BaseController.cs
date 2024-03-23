using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Resources;
using AAA.ERP.Responses;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<TEntity, TInput> : ControllerBase where TEntity : BaseEntity where TInput : BaseInputModel
    {
        private readonly IBaseService<TEntity> _service;
        private readonly BaseInputValidator<TInput> _validator;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public BaseController(IBaseService<TEntity> service, BaseInputValidator<TInput> validator, IStringLocalizer<Resource> localizer, IMapper mapper)
        {
            _service = service;
            _validator = validator;
            _mapper = mapper;
            _localizer = localizer;
        }

        protected virtual async Task<IActionResult> CreateRecord(TInput input)
        {
            var validationResult = _validator.Validate(input);
            if (validationResult.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(e => e.Type == "id")?.Value;
                var entity = _mapper.Map<TEntity>(input);
                entity.CreatedBy = Guid.Parse(userId);
                var result = await _service.Create(entity);
                result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
                return StatusCode((int)result.StatusCode, result);
            }
            else
            {
                return BadRequest(
                   new ApiResponse
                   {
                       IsSuccess = false,
                       StatusCode = HttpStatusCode.BadRequest,
                       ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
                   });
            }
        }
        protected virtual async Task<IActionResult> GetAllRecords()
        {
            var result = await _service.ReadAll();
            return StatusCode((int)result.StatusCode, result);
        }
        protected virtual async Task<IActionResult> GetRecord(Guid id)
        {
            var result = await _service.ReadById(id);
            return StatusCode((int)result.StatusCode, result);
        }
        protected virtual async Task<IActionResult> UpdateRecord(Guid id, TInput input)
        {
            var validationResult = _validator.Validate(input);
            if (validationResult.IsValid)
            {
                var entity = _mapper.Map<TEntity>(input);
                entity.Id = id;
                var userId = User.Claims.FirstOrDefault(e => e.Type == "id").Value;
                entity.ModifiedBy = Guid.Parse(userId);
                var result = await _service.Update(entity);
                return StatusCode((int)result.StatusCode, result);
            }
            else
            {
                return BadRequest(
                   new ApiResponse
                   {
                       IsSuccess = false,
                       StatusCode = HttpStatusCode.BadRequest,
                       ErrorMessages = validationResult.Errors.Select(e => _localizer[e.ErrorMessage].Value).ToList(),
                   });
            }
        }
        protected virtual async Task<IActionResult> DeleteRecord(Guid id)
        {
            var result = await _service.Delete(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
