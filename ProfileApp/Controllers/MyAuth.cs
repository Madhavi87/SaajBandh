using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProfileApp.Controllers
{
    public class MyAuth:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext c)
        {

            if (HttpContext.Current.Session["Email"] == null)
            {
                c.Result = new RedirectResult("/Account/Login");
            }

            string role = "";
            if (HttpContext.Current.Session["Role"] != null)
            {
                 role= HttpContext.Current.Session["Role"].ToString();
            }
           

            if (!Roles.Contains(role))
            {
                c.Result = new RedirectResult("/Error/ErrMsg");
            }
            
        }
    }
}