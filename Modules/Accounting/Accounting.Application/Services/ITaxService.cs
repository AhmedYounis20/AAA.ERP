using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using ERP.Application.Services.Account.SubLeadgers;

namespace Accounting.Application.Services;

public interface ITaxService : IBaseSubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>;
