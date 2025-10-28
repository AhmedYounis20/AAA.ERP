using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Mapster;

namespace ERP.Infrastracture.Services.Inventory;
public class SizeService :
    BaseSettingService<Size, SizeCreateCommand, SizeUpdateCommand>, ISizeService
{
    private readonly ISizeRepository _repository;
    public SizeService(ISizeRepository repository) : base(repository)
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
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            response.IsSuccess = false;
            response.Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } };
        }
        return response;
    }

    public override async Task<ApiResponse<Size>> Create(SizeCreateCommand command, bool isValidate = true)
    {
        try
        {
            if (isValidate)
            {
                var bussinessValidationResult = await ValidateCreate(command);
                if (!bussinessValidationResult.isValid)
                {
                    return new ApiResponse<Size>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                    };
                }
            }

            Size entity = command.Adapt<Size>();
            var nextCodeResponse = await GetNextCodeAsync();
            if (nextCodeResponse.IsSuccess)
            {
                entity.Code = nextCodeResponse.Result;
            }
            else
            {
                return new ApiResponse<Size>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = nextCodeResponse.Errors
                };
            }

            await _repository.Add(entity);
            return new ApiResponse<Size>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Size>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }
}