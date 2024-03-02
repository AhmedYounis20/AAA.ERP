using AAA.ERP.Responses;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;

namespace AAA.ERP.Services.BaseServices.impelemtation;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;
    private readonly IBaseBussinessValidator<TEntity> _bussinessValidator;
    public BaseService(IBaseRepository<TEntity> repository, IBaseBussinessValidator<TEntity> bussinessValidator)
    {
        _repository = repository;
        _bussinessValidator = bussinessValidator;
    }

    public virtual async Task<ApiResponse> Create(TEntity entity)
    {
        try
        {
            var bussinessValidationResult = await _bussinessValidator.ValidateCreateBussiness(entity);
            if (!bussinessValidationResult.IsValid)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.ListOfErrors
                };
            }

            await _repository.Add(entity);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Create(List<TEntity> entities)
    {
        try
        {
            await _repository.Add(entities);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Delete(TEntity entity)
    {
        try
        {
            await _repository.Delete(entity);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Delete(List<TEntity> entities)
    {
        try
        {
            await _repository.Delete(entities);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Delete(Guid id)
    {
        try
        {
            await _repository.Delete(id);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = id
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> ReadAll()
    {
        try
        {
            var entities = await _repository.Get();
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> ReadById(Guid id)
    {
        try
        {
            var entity = await _repository.Get(id);

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Update(TEntity entity)
    {
        try
        {
            var bussinessValidationResult = await _bussinessValidator.ValidateUpdateBussiness(entity);
            if (!bussinessValidationResult.IsValid)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = bussinessValidationResult.ListOfErrors
                };
            }

            await _repository.Update(entity);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse> Update(List<TEntity> entities)
    {
        try
        {
            await _repository.Update(entities);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}