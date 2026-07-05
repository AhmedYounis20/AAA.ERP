using Accounting.Application.Commands.CashInBoxes;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Services;

public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>;
