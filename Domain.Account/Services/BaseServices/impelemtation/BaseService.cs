using System.Collections;
using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _repository;
    private readonly IBaseBussinessValidator<TEntity> _bussinessValidator;
    public BaseService(IBaseRepository<TEntity> repository, IBaseBussinessValidator<TEntity> bussinessValidator)
    {
        _repository = repository;
        _bussinessValidator = bussinessValidator;
    }

    public virtual async Task<ApiResponse<TEntity>> Create(TEntity entity, bool isValidate = true)
    {
        try
        {
            if (isValidate)
            {
                var bussinessValidationResult = await _bussinessValidator.ValidateCreateBussiness(entity);
                if (!bussinessValidationResult.IsValid)
                {
                    return new ApiResponse<TEntity>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = bussinessValidationResult.ListOfErrors
                    };
                }
            }
            await _repository.Add(entity);
            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> Create(List<TEntity> entities, bool isValidate = true)
    {
        try
        {
            await _repository.Add(entities);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> Delete(TEntity entity, bool isValidate = true)
    {
        try
        {
            await _repository.Delete(entity);
            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> Delete(List<TEntity> entities, bool isValidate = true)
    {
        try
        {
            await _repository.Delete(entities);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true)
    {
        try
        {
            
            var removedEntity = await _repository.Delete(id);
            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = removedEntity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> ReadAll()
    {
        try
        {
            var entities = await _repository.Get();
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> ReadById(Guid id)
    {
        try
        {
            var entity = await _repository.Get(id);

            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> Update(TEntity entity, bool isValidate = true)
    {
        try
        {
            if (isValidate)
            {
                var bussinessValidationResult = await _bussinessValidator.ValidateUpdateBussiness(entity);
                if (!bussinessValidationResult.IsValid)
                {
                    return new ApiResponse<TEntity>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = bussinessValidationResult.ListOfErrors
                    };
                }
            }

            await _repository.Update(entity);
            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> Update(List<TEntity> entities, bool isValidate = true)
    {
        try
        {
            await _repository.Update(entities);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}