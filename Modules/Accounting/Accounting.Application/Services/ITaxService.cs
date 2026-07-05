using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using Accounting.Application.Services.SubLeadgers;

namespace Accounting.Application.Services;

public interface ITaxService : IBaseSubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>;
