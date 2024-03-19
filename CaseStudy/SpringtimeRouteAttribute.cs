using Microsoft.AspNetCore.Mvc;

namespace CaseStudy
{
    public class SpringtimeRouteAttribute: RouteAttribute
    {
        public SpringtimeRouteAttribute(): base("api/[controller]")
        {
        }
    }
}
