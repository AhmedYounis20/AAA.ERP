using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Mapster;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseService<TEntity, TCreateCommand, TUpdateCommand> :
    IBaseService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseEntity
    where TCreateCommand : BaseCreateCommand<TEntity>
    where TUpdateCommand : BaseUpdateCommand<TEntity>
{
    private readonly IBaseRepository<TEntity> _repository;

    public BaseService(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ApiResponse<TEntity>> Create(TCreateCommand command, bool isValidate = true)
    {
        try
        {
            if (isValidate)
            {
                var bussinessValidationResult = await ValidateCreate(command);
                if (!bussinessValidationResult.isValid)
                {
                    return new ApiResponse<TEntity>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = bussinessValidationResult.errors
                    };
                }
            }

            TEntity entity = command.Adapt<TEntity>();
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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true)
    {
        try
        {
            TEntity? entity = null;
            if (isValidate)
            {
                var validationResult = await ValidateDelete(id);
                if (!validationResult.isValid)
                {
                    return new ApiResponse<TEntity>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = validationResult.errors
                    };
                }

                entity = validationResult.entity;
            }
            else
                entity = await _repository.Get(id);

            if(entity != null)
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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
            };
        }
    }

    public virtual async Task<ApiResponse<TEntity>> Update(TUpdateCommand command, bool isValidate = true)
    {
        try
        {
            TEntity? entity = null;
            if (isValidate)
            {
                var bussinessValidationResult = await ValidateUpdate(command);
                if (!bussinessValidationResult.isValid)
                {
                    return new ApiResponse<TEntity>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = bussinessValidationResult.errors
                    };
                }

                entity = bussinessValidationResult.entity;
            }
            else
                entity = await _repository.Get(command.Id);

            if (entity != null)
            {
                entity = command.Adapt<TEntity>();
                entity.ModifiedAt = DateTime.Now;
                await _repository.Update(entity);
            }

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
                ErrorMessages = new List<string> {ex.Message}
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
                ErrorMessages = new List<string> {ex.Message}
            };
        }
    }

    protected virtual async Task<(bool isValid, List<string> errors)> ValidateCreate(TCreateCommand command)
    {
        await Task.CompletedTask;
        return (true, new List<string>());
    }

    protected virtual async Task<(bool isValid, List<string> errors,TEntity? entity)> ValidateUpdate(TUpdateCommand command)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = await _repository.Get(command.Id);
        if(entity == null)
        {
            isValid = false;
            listOfErrors = new List<string> { $"{typeof(TEntity).Name} with Id: {command.Id} not found"};
            return (isValid, listOfErrors, entity);
        }

        await Task.CompletedTask;
        return (isValid, listOfErrors, entity);
    }

    protected virtual async Task<(bool isValid, List<string> errors,TEntity? entity)> ValidateDelete(Guid id)
    {
        TEntity? entity = await _repository.Get(id);
        if (entity is null)
            return (false, ["RecordNotFound"], null);
        
        return (true, [],entity);
    }
}