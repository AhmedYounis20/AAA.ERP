using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for tax.
/// </summary>
public interface ITaxService : IBaseSubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>;
