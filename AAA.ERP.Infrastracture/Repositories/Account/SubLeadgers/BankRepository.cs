using Domain.Account.Models.Entities.SubLeadgers;
using ERP.Application.Repositories.SubLeadgers;

namespace ERP.Infrastracture.Repositories.Account.SubLeadgers;

public class BankRepository : BaseSubLeadgerRepository<Bank>, IBankRepository
{
    public BankRepository(ApplicationDbContext context) : base(context) { }
}