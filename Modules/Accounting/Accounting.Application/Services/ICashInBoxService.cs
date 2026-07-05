using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for cash in box.
/// </summary>
public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>;
