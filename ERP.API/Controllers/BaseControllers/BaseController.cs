﻿using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace ERP.API.Controllers.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<TEntity, TCreate, TUpdate> : ControllerBase
        where TEntity : BaseEntity
        where TCreate : BaseCreateCommand<TEntity>
        where TUpdate : BaseUpdateCommand<TEntity>
    {
        private readonly IBaseService<TEntity, TCreate, TUpdate> _service;
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ISender _sender;
        public string CurrentLanguage => HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
            HttpContext.Request.Headers["Accept-Language"].Any(e => e.Contains("ar")) ||
            HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
            HttpContext.Request.Headers["Accept-Culture"].Any(e => e.Contains("ar")) ? "ar" : "en";


        public BaseController(IBaseService<TEntity, TCreate, TUpdate> service,
            IStringLocalizer<Resource> localizer,
            ISender sender)
        {
            _service = service;
            _localizer = localizer;
            _sender = sender;
        }

        protected virtual async Task<IActionResult> CreateRecord(TCreate input)
        {
            var result = await _sender.Send(input);
            result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
            if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
                result.SuccessMessage = _localizer["CreatedSuccessfully"].Value;
            return StatusCode((int)result.StatusCode, result);
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

        protected virtual async Task<IActionResult> UpdateRecord(Guid id, TUpdate input)
        {
            input.Id = id;
            var result = await _sender.Send(input);
            result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
              if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
                result.SuccessMessage = _localizer["UpdatedSuccessfully"].Value;

            return StatusCode((int)result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> DeleteRecord(Guid id)
        {
            var result = await _service.Delete(id);
            result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
            if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.SuccessMessage = _localizer["DeletedSuccessfully"].Value;

            return StatusCode((int)result.StatusCode, result);
        }
    }
}