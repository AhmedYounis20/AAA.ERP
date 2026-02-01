using ERP.Domain.Commands.Account.SubLeadgers.Taxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Application.Services.Account.SubLeadgers;

public interface ITaxService : IBaseSubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>
{ }
