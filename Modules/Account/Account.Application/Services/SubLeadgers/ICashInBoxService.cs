using Account.Domain.Commands.SubLeadgers.CashInBoxes;
using Account.Domain.Models.Entities.SubLeadgers;


namespace Account.Application.Services.SubLeadgers;

public interface ICashInBoxService : IBaseSubLeadgerService<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>
{ }