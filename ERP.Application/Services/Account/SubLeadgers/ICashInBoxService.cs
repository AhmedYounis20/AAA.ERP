using ERP.Domain.Commands.Account.SubLeadgers.CashInBoxes;
using ERP.Domain.Models.Entities.Account.SubLeadgers;


namespace ERP.Application.Services.Account.SubLeadgers;

public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>
{ }