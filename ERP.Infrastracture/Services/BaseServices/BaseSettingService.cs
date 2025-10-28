using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace ERP.Infrastracture.Services.BaseServices;

public class BaseSettingService<TEntity, TCreateCommand, TUpdateCommand>
    : BaseService<TEntity, TCreateCommand, TUpdateCommand>, IBaseSettingService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseSettingEntity
    where TCreateCommand : BaseSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseSettingUpdateCommand<TEntity>
{
    private readonly IBaseSettingRepository<TEntity> _repository;

    public BaseSettingService(IBaseSettingRepository<TEntity> repository) : base(repository)
        => _repository = repository;

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string name)
    {
        try
        {
            var entities = await _repository.Search(name);
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

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(TCreateCommand command)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = null;

        var existedEntity = await _repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity != null)
        {
            isValid = false;

            if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
                listOfErrors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
                listOfErrors.Add("WithSameNameSecondLanguageIsExisted");
        }

        return (isValid, listOfErrors);
    }

    protected override async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateUpdate(TUpdateCommand command)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = null;

        var oldEntity = await _repository.Get(command.Id);
        var existedEntity = await _repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity != null && existedEntity.Id != command.Id)
        {
            isValid = false;
            if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
                listOfErrors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
                listOfErrors.Add("WithSameNameSecondLanguageIsExisted");
        }

        entity = oldEntity;
        return (isValid, listOfErrors, entity);
    }
}