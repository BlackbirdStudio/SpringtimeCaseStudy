using AutoMapper;
using CaseStudy.EFCore;

namespace CaseStudy.Controllers
{
    public class ContactPersonController : CrudController<EFCore.Models.ContactPerson, Models.ContactPerson>
    {
        public ContactPersonController(Repository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
