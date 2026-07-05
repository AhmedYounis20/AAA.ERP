using Accounting.Application.Commands.Branches;
using Accounting.Application.Services.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;

namespace Accounting.Application.Services;

public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>;
