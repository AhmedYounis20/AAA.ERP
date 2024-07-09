﻿using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Services.BaseServices.interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Shared.BaseEntities;
using Shared.Resources;

namespace AAA.ERP.Controllers.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<TEntity, TCreate, TUpdate> : ControllerBase
        where TEntity : BaseEntity
        where TCreate : BaseCreateCommand<TEntity>
        where TUpdate : BaseUpdateCommand<TEntity>
    {
        private readonly IBaseService<TEntity,TCreate,TUpdate> _service;
        private readonly IStringLocalizer<Resource> _localizer;
        private readonly ISender _sender;
        public string CurrentLanguage => ((HttpContext.Request.Headers.ContainsKey("Accept-Language") &&
            HttpContext.Request.Headers["Accept-Language"].Contains("ar")) ||
            (HttpContext.Request.Headers.ContainsKey("Accept-Culture") &&
            HttpContext.Request.Headers["Accept-Culture"].Contains("ar"))) ? "ar" : "en";


        public BaseController(IBaseService<TEntity,TCreate,TUpdate> service,
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
            return StatusCode((int) result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> GetAllRecords()
        {
            var result = await _service.ReadAll();
            return StatusCode((int) result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> GetRecord(Guid id)
        {
            var result = await _service.ReadById(id);
            return StatusCode((int) result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> UpdateRecord(Guid id, TUpdate input)
        {
            var result = await _sender.Send(input);
            result.ErrorMessages = result.ErrorMessages?.Select(e => _localizer[e].Value).ToList();
            return StatusCode((int) result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> DeleteRecord(Guid id)
        {
            var result = await _service.Delete(id);
            return StatusCode((int) result.StatusCode, result);
        }
    }
}