using ERP.Application.Repositories.Inventory;
using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.Colors;
using ERP.Infrastracture.Services.BaseServices;

namespace Inventory.Infrastructure.Services;

public class ColorService
    : BaseSettingService<Color, ColorCreateCommand, ColorUpdateCommand>, IColorService
{
    private readonly IColorRepository _repository;

    public ColorService(IColorRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<string>> GetNextCodeAsync()
    {
        var response = new ApiResponse<string>();
        try
        {
            var maxCode = await _repository.GetMaxCodeAsync();
            int next = 1;
            if (int.TryParse(maxCode, out int max))
                next = max + 1;
            response.Result = next.ToString();
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.IsSuccess = false;
            response.Errors = [new MessageTemplate { MessageKey = ex.Message }];
        }
        return response;
    }

    public override async Task<ApiResponse<Color>> Create(ColorCreateCommand command, bool isValidate = true)
    {
        try
        {
            if (isValidate)
            {
                var bussinessValidationResult = await ValidateCreate(command);
                if (!bussinessValidationResult.isValid)
                {
                    return new ApiResponse<Color>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                    };
                }
            }

            Color entity = command.Adapt<Color>();
            var nextCodeResponse = await GetNextCodeAsync();
            if (nextCodeResponse.IsSuccess)
            {
                entity.Code = nextCodeResponse.Result;
            }
            else
            {
                return new ApiResponse<Color>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = nextCodeResponse.Errors
                };
            }

            await _repository.Add(entity);
            return new ApiResponse<Color>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Color>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(ColorCreateCommand command)
    {
        var (isValid, errors) = await base.ValidateCreate(command);

        var colorValueExists = await _repository.GetByColorValueExists(command.ColorValue);
        if (colorValueExists)
        {
            isValid = false;
            errors.Add("ColorValueIsDuplicate");
        }

        return (isValid, errors);
    }
}
