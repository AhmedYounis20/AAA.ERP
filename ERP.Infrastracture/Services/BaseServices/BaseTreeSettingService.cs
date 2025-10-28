using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace ERP.Infrastracture.Services.BaseServices;

public class BaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    : BaseSettingService<TEntity, TCreateCommand, TUpdateCommand>,
        IBaseTreeSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreateCommand : BaseTreeSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseTreeSettingUpdateCommand<TEntity>
{
    private readonly IBaseTreeSettingRepository<TEntity> _repository;

    public BaseTreeSettingService(IBaseTreeSettingRepository<TEntity> repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<List<TEntity>> GetChildren(Guid id, int level = 0)
        => _repository.GetChildren(id, level);

    public Task<List<TEntity>> GetLevel(int level = 0)
        => _repository.GetLevel(level);

    public override async Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await ValidateDelete(id);
        if (!validationResult.isValid)
        {
            return new ApiResponse<TEntity>
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        return await base.Delete(id, isValidate);
    }

    protected override async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateDelete(Guid id)
    {
        bool isParent = await _repository.HasChildren(id);
        if (isParent)
        {
            return (false, new List<string> { "CannotDeleteParent" }, null);
        }

        return await base.ValidateDelete(id);
    }
}