using AutoMapper;
using CaseStudy.EFCore;
namespace CaseStudy.Controllers
{
    public class BankAccountController : CrudController<EFCore.Models.BankAccount, Models.BankAccount>
    {
        public BankAccountController(Repository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
