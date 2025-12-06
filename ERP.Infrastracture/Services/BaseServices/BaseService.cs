using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.DTOs;
using System.Linq.Expressions;

namespace ERP.Infrastracture.Services.BaseServices;

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
                        Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                        Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                    };
                }

                entity = validationResult.entity;
            }
            else
                entity = await _repository.Get(id);

            if (entity != null)
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public virtual async Task<ApiResponse<PaginatedResult<TEntity>>> ReadAllPaginated(
        BaseFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        return await ReadAllPaginated(filter, null, cancellationToken);
    }

    public virtual async Task<ApiResponse<PaginatedResult<TEntity>>> ReadAllPaginated(
        BaseFilterDto filter,
        Expression<Func<TEntity, bool>>? additionalFilter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Build filter expression
            Expression<Func<TEntity, bool>>? filterExpression = additionalFilter;

            // Add date range filter if specified
            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = CombineFilters(filterExpression, e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value));
            }

            // Determine sort expression
            Expression<Func<TEntity, object>>? orderBy = GetOrderByExpression(filter.SortBy);

            var result = await _repository.GetPaginated(
                filter.PageNumber,
                filter.PageSize,
                filterExpression,
                orderBy,
                filter.SortDescending,
                cancellationToken);

            return new ApiResponse<PaginatedResult<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    /// <summary>
    /// Override this method to provide custom sorting for entity-specific properties
    /// </summary>
    protected virtual Expression<Func<TEntity, object>>? GetOrderByExpression(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return e => e.CreatedAt;

        return sortBy.ToLower() switch
        {
            "createdat" => e => e.CreatedAt,
            "modifiedat" => e => e.ModifiedAt,
            _ => e => e.CreatedAt
        };
    }

    /// <summary>
    /// Combines two filter expressions with AND logic
    /// </summary>
    protected static Expression<Func<TEntity, bool>>? CombineFilters(
        Expression<Func<TEntity, bool>>? first,
        Expression<Func<TEntity, bool>> second)
    {
        if (first == null)
            return second;

        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var combined = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter));

        return Expression.Lambda<Func<TEntity, bool>>(combined, parameter);
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                        Errors = bussinessValidationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
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
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    protected virtual async Task<(bool isValid, List<string> errors)> ValidateCreate(TCreateCommand command)
    {
        await Task.CompletedTask;
        return (true, new List<string>());
    }

    protected virtual async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateUpdate(TUpdateCommand command)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = await _repository.Get(command.Id);
        if (entity == null)
        {
            isValid = false;
            listOfErrors = new List<string> { $"{typeof(TEntity).Name} with Id: {command.Id} not found" };
            return (isValid, listOfErrors, entity);
        }

        return (isValid, listOfErrors, entity);
    }

    protected virtual async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateDelete(Guid id)
    {
        TEntity? entity = await _repository.Get(id);
        if (entity is null)
            return (false, ["RecordNotFound"], null);

        return (true, [], entity);
    }
}