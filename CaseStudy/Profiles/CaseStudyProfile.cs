using AutoMapper;

namespace CaseStudy.Profiles
{
    public class CaseStudyProfile : Profile
    {
        public CaseStudyProfile()
        {
            MapWithIgnoredId<EFCore.Models.BankAccount>();
            MapWithIgnoredId<EFCore.Models.ContactPerson>();
            MapWithIgnoredId<EFCore.Models.Vendor>();
            CreateMap<Models.BankAccount, EFCore.Models.BankAccount>().ReverseMap();
            CreateMap<Models.ContactPerson, EFCore.Models.ContactPerson>().ReverseMap();
            CreateMap<Models.Vendor, EFCore.Models.Vendor>().ReverseMap();
        }

        private void MapWithIgnoredId<TEntity>() where TEntity : EFCore.Models.BaseEntity
            => CreateMap<TEntity, TEntity>().ForMember(o => o.Id, o => o.Ignore());
    }
}
