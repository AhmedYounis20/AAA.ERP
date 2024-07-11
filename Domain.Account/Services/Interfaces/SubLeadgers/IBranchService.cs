using Domain.Account.Commands.SubLeadgers.CashInBoxes;
using Domain.Account.Models.Entities.SubLeadgers;


namespace AAA.ERP.Services.Interfaces.SubLeadgers;

public interface IBranchService : IBaseSubLeadgerService<Branch,BranchCreateCommand,BranchUpdateCommand>
{ }