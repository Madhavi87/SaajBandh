using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ProfileApp.Controllers
{
    public class AdminAuth:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext c)
        {
            string role = "";
            if (HttpContext.Current.Session["AdminUser"] == null)
            {
                //c.Result = new RedirectResult("/AdminUser/Login");
                c.Result = new RedirectToRouteResult(
                new RouteValueDictionary 
                {
                    { "action", "Login" },
                    { "controller", "AdminUser" }
                });
                
            }

           
            if (HttpContext.Current.Session["Role"] != null)
            {
                role = HttpContext.Current.Session["Role"].ToString();
            }
           
            if (!Roles.Contains(role))
            {
                c.Result = new RedirectResult("/Error/ErrMsg");
            }

        }
    }
}