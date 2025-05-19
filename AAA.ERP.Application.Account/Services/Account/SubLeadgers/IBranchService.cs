using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;


namespace ERP.Application.Services.Account.SubLeadgers;

public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>
{ }