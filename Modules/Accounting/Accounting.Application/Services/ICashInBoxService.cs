using Accounting.Application.Commands.CashInBoxes;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>;
