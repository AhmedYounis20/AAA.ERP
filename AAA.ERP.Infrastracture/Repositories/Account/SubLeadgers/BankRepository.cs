using ERP.Application.Repositories.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Account.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class BankRepository : BaseSubLeadgerRepository<Bank>, IBankRepository
{
    public BankRepository(ApplicationDbContext context) : base(context) { }
}