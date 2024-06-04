using AAA.ERP.Services.Interfaces.SubLeadgers;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

namespace AAA.ERP.Services.Impelementation.SubLeadgers;

public class CashInBoxService : BaseTreeSettingService<CashInBox>, ICashInBoxService
{
    public CashInBoxService(
        ICashInBoxRepository repository,
        IBaseTreeSettingBussinessValidator<CashInBox> bussinessValidator
    ) : base(repository,
        bussinessValidator)
    {}
    
}