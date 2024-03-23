using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

namespace AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;

public class BaseBussinessValidator<TEntity> : IBaseBussinessValidator<TEntity> where TEntity : BaseEntity
{
    private IBaseRepository<TEntity> _repository;
    public BaseBussinessValidator(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    virtual public async Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateCreateBussiness(TEntity inpuModel)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = null;
        await Task.CompletedTask;

        return (isValid, listOfErrors, entity);
    }

    virtual public async Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateUpdateBussiness(TEntity inpuModel)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = await _repository.Get(inpuModel.Id);
        if(entity == null)
        {
            isValid = false;
            listOfErrors = new List<string> { $"{typeof(TEntity).Name} with Id: {inpuModel} not found"};
            return (isValid, listOfErrors, entity);
        }


        await Task.CompletedTask;
        return (isValid, listOfErrors, entity);
    }
}
