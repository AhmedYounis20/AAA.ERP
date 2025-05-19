using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;


namespace ERP.Application.Services.Account.SubLeadgers;

public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>
{ }