using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Gargantuan.Tarantula.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        #region "Authentication Functions"
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Authenticate(FormCollection body)
        {
            //this.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(body["Name"]),new string[] { });
            DateTime expire = DateTime.Now.AddMinutes(15);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "cookie", DateTime.Now, expire, true, body["Name"]);
            HttpCookie cookie = new HttpCookie("ChocolateChip");
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(cookie);

            string redirectUrl = Request.QueryString.AllKeys.Contains("returnUrl") ? Request.QueryString["returnUrl"] : "Index";



            return new RedirectResult(redirectUrl);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            HttpCookie cookie = new HttpCookie("ChocolateChip");

            Response.SetCookie(cookie);

            return new RedirectResult("Index");
        }

        #endregion

    }
}