using Accounting.Application.Commands.Branches;
using ERP.Application.Services.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace Accounting.Application.Services;

public interface IBranchService : IBaseSubLeadgerService<Branch, BranchCreateCommand, BranchUpdateCommand>;
