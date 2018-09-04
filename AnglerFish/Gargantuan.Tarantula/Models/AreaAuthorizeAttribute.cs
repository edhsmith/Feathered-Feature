using System.Linq;
using System.Web.Mvc;

namespace Gargantuan.Tarantula.Models
{
    public class AreaAuthorizeAttribute: AuthorizeAttribute
    {
        public AreaAuthorizeAttribute()
        {

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Get Area
            string area = filterContext.RouteData.DataTokens.Keys.Contains("area")
                      ? filterContext.RouteData.DataTokens["area"].ToString()
                      : string.Empty;
            //Get Controller
            string controller = filterContext.RouteData.Values["controller"].ToString();
            
            //Login Url
            string loginUrl = string.Format("~/{0}/{1}/Login", area, controller);

            filterContext.Result = new RedirectResult(string.Concat(loginUrl, "?returnUrl=", filterContext.HttpContext.Request.Url.PathAndQuery));
        }
    }
}