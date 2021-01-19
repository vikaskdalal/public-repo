
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingSite.Models
{
    public class AccessDeniedAuthorizationAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Users", Action = "AccessDenied" }));
            }
        }
    }
    
}