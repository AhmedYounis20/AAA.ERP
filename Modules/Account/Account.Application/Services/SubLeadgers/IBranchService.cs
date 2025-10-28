using Account.Domain.Commands.SubLeadgers.Branches;
using Account.Domain.Models.Entities.SubLeadgers;

namespace Account.Application.Services.SubLeadgers;

public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>
{ }