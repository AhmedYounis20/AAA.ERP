using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using Accounting.Application.Services.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for tax.
/// </summary>
public interface ITaxService : IBaseSubLeadgerService<Tax, TaxCreateCommand, TaxUpdateCommand>;
