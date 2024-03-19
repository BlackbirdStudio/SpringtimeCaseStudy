using Microsoft.AspNetCore.Authorization;

namespace CaseStudy
{
    public class AuthorizeRoleManageDataAttribute: AuthorizeAttribute
    {
        public AuthorizeRoleManageDataAttribute()
        {
            Roles = CaseStudy.Roles.ManageData;
        }
    }
}
