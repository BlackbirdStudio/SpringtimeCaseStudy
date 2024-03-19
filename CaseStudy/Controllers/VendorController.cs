using AutoMapper;
using CaseStudy.EFCore;

namespace CaseStudy.Controllers
{
    public class VendorController : CrudController<EFCore.Models.Vendor, Models.Vendor>
    {
        public VendorController(Repository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
