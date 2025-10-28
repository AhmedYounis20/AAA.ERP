using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
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
        private readonly ISender _sender;
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

        protected virtual async Task<IActionResult> CreateRecord(TCreate input)
        {
            var result = await _sender.Send(input);
            if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
                result.Success = new MessageTemplate { MessageKey = "CreatedSuccessfully" };
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
            if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
                result.Success = new MessageTemplate { MessageKey = "UpdatedSuccessfully" };
            return StatusCode((int)result.StatusCode, result);
        }

        protected virtual async Task<IActionResult> DeleteRecord(Guid id)
        {
            var result = await _service.Delete(id);
            if (result.IsSuccess && string.IsNullOrEmpty(result.SuccessMessage))
            result.Success = new MessageTemplate { MessageKey = "DeletedSuccessfully" };
            return StatusCode((int)result.StatusCode, result);
        }
    }
}