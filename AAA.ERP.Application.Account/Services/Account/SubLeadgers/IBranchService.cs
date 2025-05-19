using ERP.Domain.Commands.Account.SubLeadgers.Branches;
using ERP.Domain.Models.Entities.Account.SubLeadgers;


namespace ERP.Application.Services.Account.SubLeadgers;

public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>
{ }