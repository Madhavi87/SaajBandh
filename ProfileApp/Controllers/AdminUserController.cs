using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileApp.Models;
using DAL;
using System.Web.Security;
namespace ProfileApp.Controllers
{
    public class AdminUserController : Controller
    {
        ProfileEntities db = new ProfileEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLog obj)
        {
            var data = db.AdminLogins.Where(x => x.UserName == obj.UserName && x.Password == obj.Password).FirstOrDefault();
            if (data != null)
            {
                Session["AdminUserID"] = data.UserID;
                Session["AdminUser"] = data.UserName;
                Session["Role"] = "Admin";
                FormsAuthentication.SetAuthCookie(data.UserName, true);

                return RedirectToAction("ActiveUsers");
            }
            else
            {
                ModelState.AddModelError("ErrMsg", "Invalid Credentials");
                return View(obj);
            }
        }

        //public ActionResult Logout()
        //{
        //    Session["AdminUserID"] = null;
        //    Session["AdminUser"] = null;

        //    return RedirectToAction("Login");
        //}


        public ActionResult AdminHome()
        {
            return View();
        }

        [AdminAuth(Roles = "Admin")]
        public ActionResult ActiveUsers()
        {
            var userList = (from context in db.UserRegistrations
                            join userprofile in db.UserProfiles on context.UserID equals userprofile.UserID
                            where context.IsActive == true
                            select (new UserProfileVM
                            {
                                FirstName = context.FirstName,
                                MiddleName = context.MiddleName,
                                LastName = context.LastName,
                                Email = context.Email,
                                Mobile = context.Mobile,
                                UserID = context.UserID,
                                MyAppID = userprofile.MyAppID,
                                Password = context.Password

                            })).ToList();



            //List<UserProfileVM> lst = new List<ActiveDeactiveUser>();

            //var data = db.UserRegistrations.Where(x => x.IsActive == true).ToList();
            //foreach (var i in data)
            //{
            //    ActiveDeactiveUser a = new ActiveDeactiveUser();

            //    a.UserID = i.UserID;
            //    a.FirstName = i.FirstName;
            //    a.MiddleName = i.MiddleName;
            //    a.LastName = i.LastName;
            //    a.Email = i.Email;
            //    a.Mobile = i.Mobile;
            //    a.IsActive = false;

            //    lst.Add(a);
            //}
            return View(userList);
        }

        [AdminAuth(Roles = "Admin")]
        public ActionResult DeActiveUsers()
        {
            var userList = (from context in db.UserRegistrations
                            join userprofile in db.UserProfiles on context.UserID equals userprofile.UserID
                            where context.IsActive == false
                            select (new UserProfileVM
                            {
                                FirstName = context.FirstName,
                                MiddleName = context.MiddleName,
                                LastName = context.LastName,
                                Email = context.Email,
                                Mobile = context.Mobile,
                                UserID = context.UserID,
                                MyAppID = userprofile.MyAppID,
                                Password = context.Password

                            })).ToList();

            //    List<UserProfileVM> lst = new List<ActiveDeactiveUser>();

            //    var data = db.UserRegistrations.Where(x => x.IsActive == false).ToList();
            //    foreach (var i in data)
            //    {
            //        ActiveDeactiveUser a = new ActiveDeactiveUser();
            //        a.UserID = i.UserID;

            //        a.FirstName = i.FirstName;
            //        a.LastName = i.LastName;
            //        a.MiddleName = i.MiddleName;
            //        a.Email = i.Email;
            //        a.Mobile = i.Mobile;

            //        a.IsActive = false;

            //        lst.Add(a);
            //    }
            return View(userList);
        }


        [HttpPost]
        public ActionResult ActiveUsers(List<UserProfileVM> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].IsActive == true)
                {
                    UserRegistration a = db.UserRegistrations.Find(lst[i].UserID);
                    a.IsActive = false;
                    db.Entry(a).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

            }


            // List<ActiveDeactiveUser> list1 = new List<ActiveDeactiveUser>();

            // var data = db.UserRegistrations.Where(x => x.IsActive == true).ToList();
            //foreach (var i in data)
            //{
            //    ActiveDeactiveUser a = new ActiveDeactiveUser();
            //    a.UserID = i.UserID;
            //    a.FullName = i.FullName;
            //    a.Email = i.Email;
            //    a.Mobile = i.Mobile;
            //    a.City = i.City;
            //    a.IsActive = false;

            //    list1.Add(a);
            //}


            return RedirectToAction("ActiveUsers");
        }

        [HttpPost]
        public ActionResult DeActiveUsers(List<UserProfileVM> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].IsActive == true)
                {
                    UserRegistration a = db.UserRegistrations.Find(lst[i].UserID);
                    a.IsActive = true;
                    db.Entry(a).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

            }




            return RedirectToAction("DeActiveUsers");
        }


        public ActionResult DeleteUser(int iUserID)
        {
            UserRegistration a = db.UserRegistrations.Find(iUserID);
            db.UserRegistrations.Remove(a);
            db.SaveChanges();
            return RedirectToAction("ActiveUsers");
        }

        public ActionResult Logout()
        {

            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
