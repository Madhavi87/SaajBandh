using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DAL;
using ProfileApp.Models;
using System.Text;
using System.IO;
using System.Web.UI;

namespace ProfileApp.Controllers
{

    [Authorize]
    public class SearchUserController : Controller
    {

        //public static string RenderPartialToString(string controlName, object viewData)
        //{
        //    ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

        //    viewPage.ViewData = new ViewDataDictionary(viewData);
        //    viewPage.Controls.Add(viewPage.LoadControl(controlName));

        //    StringBuilder sb = new StringBuilder();
        //    using (StringWriter sw = new StringWriter(sb))
        //    {
        //        using (HtmlTextWriter tw = new HtmlTextWriter(sw))
        //        {
        //            viewPage.RenderControl(tw);
        //        }
        //    }

        //    return sb.ToString();
        //}

        protected string RenderPartialViewToString(string viewName, object model)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetNoStore();

            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        ProfileEntities db = new ProfileEntities();


        public ActionResult SearchByID()
        {
            return View("QuickSearch");
        }


        public ActionResult DisplaySearch(string id)
        {
            var data = db.SPQuickSearch(id).FirstOrDefault();
            ShowProfile obj = new ShowProfile();

            if (data != null)
            {
                obj.UserID = data.UserID.Value;
                if (data.DateOfBirth == null)
                {
                    obj.DateOfBirth = DateTime.Now;
                }
                else
                {
                    obj.DateOfBirth = data.DateOfBirth.Value;
                }

                var UserProfile = db.UserRegistrations.Where(x => x.UserID == data.UserID).FirstOrDefault();
                obj.Work = data.IndustryName;
                obj.Gender = data.GenderName;
                obj.MritalStatus = data.Status;
                obj.RAddress = data.RAddress;
                obj.FirstName = UserProfile.FirstName;
                obj.LastName = UserProfile.LastName;
                obj.MiddleName = UserProfile.MiddleName;
                obj.City = data.City;
                obj.Email = data.Email;
                obj.Mobile = data.Mobile;
                obj.BloodGroup = data.BloodG;
                obj.MyAppID = data.MyAppID;

                obj.ImagePath = data.ImagePath;

            }

            return PartialView("ucSearch", obj);
        }


        public ActionResult AdvanceSearch()
        {
            ViewBag.Genders = db.Genders.ToList();
            ViewBag.Status = db.MaritalStatus.ToList();
            var citylist = db.Cities.Where(x => x.StateID == 15).OrderBy(x => x.CityName).ToList();
            ViewBag.Cities = citylist;
            return View();
        }


        public ActionResult ProfileInfo(int UserID)
        {
            var data = db.Get_Profile(UserID).FirstOrDefault();
            ShowProfile obj = new ShowProfile();

            if (data != null)
            {
                obj.UserID = data.UserID;
                if (data.DateOfBirth == null)
                {
                    obj.DateOfBirth = DateTime.Now;
                }
                else
                {
                    obj.DateOfBirth = data.DateOfBirth.Value;
                }
                var UserProfile = db.UserRegistrations.Where(x => x.UserID == data.UserID).FirstOrDefault();
                obj.Work = data.IndustryName;
                obj.Gender = data.GenderName;
                obj.MritalStatus = data.Status;
                obj.RAddress = data.RAddress;

                obj.FirstName = UserProfile.FirstName;
                obj.LastName = UserProfile.LastName;
                obj.MiddleName = UserProfile.MiddleName;
                obj.City = data.CityName;
                obj.State = data.StateName;
                obj.Email = data.Email;
                obj.Mobile = data.Mobile;
                obj.BloodGroup = data.BloodG;
                obj.MyAppID = data.MyAppID;

                obj.ImagePath = data.ImagePath;

            }
            else
            {
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                obj.UserID = Convert.ToInt32(Session["UserID"].ToString());
                obj.DateOfBirth = DateTime.Now;
                obj.Work = "";
                obj.Gender = "";
                obj.MritalStatus = "";
                obj.RAddress = "";
                var UserProfile = db.UserRegistrations.Where(x => x.UserID == data.UserID).FirstOrDefault();
                obj.FirstName = UserProfile.FirstName;
                obj.LastName = UserProfile.LastName;
                obj.MiddleName = UserProfile.MiddleName;
                obj.City = db.Cities.Where(x => x.CityID == UserProfile.CityID).FirstOrDefault().CityName;
                obj.Email = db.UserRegistrations.Where(x => x.UserID == uid).FirstOrDefault().Email;
                obj.Mobile = db.UserRegistrations.Where(x => x.UserID == uid).FirstOrDefault().Mobile;
                obj.BloodGroup = "Not Available";
                obj.ImagePath = "/Images/Photo.jpg";
            }



            return PartialView("_ucProfile", obj);

        }


        public ActionResult ViewEducation(int UserID)
        {
            var data = db.Get_Education(UserID).ToList();
            return PartialView("_ucEducation", data);
        }



        public ActionResult ViewEmployment(int UserID)
        {
            var data = db.Get_Employment(UserID).ToList();
            List<EmploymentDetails> lst = new List<EmploymentDetails>();
            foreach (var i in data)
            {
                EmploymentDetails d = new EmploymentDetails();
                d.EmpDetailID = i.EmpDetailID;
                d.EmployerName = i.EmployerName;
                d.UserID = i.UserID;
                d.TypeID = i.TypeID;
                d.Type = i.Type;
                d.Designation = i.Desgignation;


                if (i.Type == "Current Employer" && i.DateTo == null)
                {
                    d.DateFrom = i.DateFrom.Value;
                    d.DateTo = null;
                    d.Gap = "From " + DateCommonFunctions.GetShortDateFormat(i.DateFrom.Value) + " To Present";

                }
                else
                {
                    d.DateFrom = i.DateFrom.Value;
                    d.DateTo = i.DateTo.Value;
                    d.Gap = "From " + DateCommonFunctions.GetShortDateFormat(i.DateFrom.Value) + " To " + DateCommonFunctions.GetShortDateFormat(i.DateTo.Value);
                }

                lst.Add(d);

            }

            return PartialView("_ucEmployment", lst);
        }


        //For Quick Search

        public ActionResult QuickProfileInfo(string UserID)
        {
            int uid = db.UserProfiles.Where(x => x.MyAppID == UserID).FirstOrDefault().UserID;
            var data = db.Get_Profile(uid).FirstOrDefault();
            ShowProfile obj = new ShowProfile();

            if (data != null)
            {
                obj.UserID = data.UserID;
                if (data.DateOfBirth == null)
                {
                    obj.DateOfBirth = DateTime.Now;
                }
                else
                {
                    obj.DateOfBirth = data.DateOfBirth.Value;
                }

                obj.Work = data.IndustryName;
                obj.Gender = data.GenderName;
                obj.MritalStatus = data.Status;
                obj.RAddress = data.RAddress;

                var UserProfile = db.UserRegistrations.Where(x => x.UserID == data.UserID).FirstOrDefault();

                obj.FirstName = UserProfile.FirstName;
                obj.LastName = UserProfile.LastName;
                obj.MiddleName = UserProfile.MiddleName;
                obj.City = data.CityName;
                obj.State = data.StateName;
                obj.Email = data.Email;
                obj.Mobile = data.Mobile;
                obj.BloodGroup = data.BloodG;
                obj.MyAppID = data.MyAppID;

                obj.ImagePath = data.ImagePath;

            }
            else
            {

                obj.UserID = Convert.ToInt32(Session["UserID"].ToString());
                obj.DateOfBirth = DateTime.Now;
                obj.Work = "";
                obj.Gender = "";
                obj.MritalStatus = "";
                obj.RAddress = "";
                var UserProfile = db.UserRegistrations.Where(x => x.UserID == data.UserID).FirstOrDefault();

                obj.FirstName = UserProfile.FirstName;
                obj.LastName = UserProfile.LastName;
                obj.MiddleName = UserProfile.MiddleName;
                obj.City = db.Cities.Where(x => x.CityID == UserProfile.CityID).FirstOrDefault().CityName;
                obj.Email = db.UserRegistrations.Where(x => x.UserID == uid).FirstOrDefault().Email;
                obj.Mobile = db.UserRegistrations.Where(x => x.UserID == uid).FirstOrDefault().Mobile;
                obj.BloodGroup = "Not Available";
                obj.ImagePath = "/Images/Photo.jpg";
            }



            return PartialView("_ucProfile", obj);

        }


        public ActionResult QuickViewEducation(string UserID)
        {
            int uid = db.UserProfiles.Where(x => x.MyAppID == UserID).FirstOrDefault().UserID;
            var data = db.Get_Education(uid).ToList();
            return PartialView("_ucEducation", data);
        }


        public ActionResult QuickViewEmployment(string UserID)
        {
            int uid = db.UserProfiles.Where(x => x.MyAppID == UserID).FirstOrDefault().UserID;
            var data = db.Get_Employment(uid).ToList();
            List<EmploymentDetails> lst = new List<EmploymentDetails>();
            foreach (var i in data)
            {
                EmploymentDetails d = new EmploymentDetails();
                d.EmpDetailID = i.EmpDetailID;
                d.EmployerName = i.EmployerName;
                d.UserID = i.UserID;
                d.TypeID = i.TypeID;
                d.Type = i.Type;
                d.Designation = i.Desgignation;


                if (i.Type == "Current Employer" && i.DateTo == null)
                {
                    d.DateFrom = i.DateFrom.Value;
                    d.DateTo = null;
                    d.Gap = "From " + DateCommonFunctions.GetShortDateFormat(i.DateFrom.Value) + " To Present";

                }
                else
                {
                    d.DateFrom = i.DateFrom.Value;
                    d.DateTo = i.DateTo.Value;
                    d.Gap = "From " + DateCommonFunctions.GetShortDateFormat(i.DateFrom.Value) + " To " + DateCommonFunctions.GetShortDateFormat(i.DateTo.Value);
                }

                lst.Add(d);

            }

            return PartialView("_ucEmployment", lst);
        }




        public JsonResult SearchUserListByID(string iUserId)
        {
            var userProfileList = db.UserProfiles.ToList();
            var userList = (from context in db.UserRegistrations
                            join userproifle in db.UserProfiles on context.UserID equals userproifle.UserID
                            where userproifle.MyAppID == iUserId
                            select (new UserProfileVM
                            {
                                FirstName = context.FirstName,
                                MiddleName = context.MiddleName,
                                LastName = context.LastName,
                                MyAppID = userproifle.MyAppID,
                                Email = context.Email,
                                BirthYear = userproifle.DateOfBirth.HasValue ? userproifle.DateOfBirth.Value.Year : 0,
                                BirthPlace = userproifle.BirthPlace,
                                BirthTime = userproifle.BirthTime,
                                Gotra = userproifle.Gotra,
                                EducationDetail = userproifle.EducationDetail,
                                Mobile = context.Mobile,
                                ImagePath = userproifle.ImagePath,
                                UserID = userproifle.UserID

                            })).ToList();



            UserProfileVM objUserProfileVM = new UserProfileVM();

            string vResult = RenderPartialViewToString("~/Views/SearchUser/ucSearchResultList.cshtml", userList);

            return Json(vResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchUserListByDetails(int iGenderID = 0, int iStatusID = 0, int iCityID = 0)
        {
            var userProfileList = db.UserProfiles.ToList();
            var userList = (from context in db.UserRegistrations
                            join userproifle in db.UserProfiles on context.UserID equals userproifle.UserID
                            where iGenderID > 0 ? userproifle.Gender.Value == iGenderID : false &&
                            iStatusID > 0 ? userproifle.MritalStatus.Value == iStatusID : false &&
                            iCityID > 0 ? userproifle.DistrictID.Value == iCityID : false
                            select (new UserProfileVM
                            {
                                FirstName = context.FirstName,
                                MiddleName = context.MiddleName,
                                LastName = context.LastName,
                                MyAppID = userproifle.MyAppID,
                                Email = context.Email,
                                BirthYear = userproifle.DateOfBirth.HasValue ? userproifle.DateOfBirth.Value.Year : 0,
                                BirthPlace = userproifle.BirthPlace,
                                BirthTime = userproifle.BirthTime,
                                Gotra = userproifle.Gotra,
                                EducationDetail = userproifle.EducationDetail,
                                Mobile = context.Mobile,
                                ImagePath = userproifle.ImagePath,
                                UserID = userproifle.UserID

                            })).ToList();



            UserProfileVM objUserProfileVM = new UserProfileVM();

            string vResult = RenderPartialViewToString("~/Views/SearchUser/ucSearchResultList.cshtml", userList);

            return Json(vResult, JsonRequestBehavior.AllowGet);
        }




        public ActionResult ViewDetailsByID(int iuserID)
        {

            return RedirectToAction("ViewProfile", "Profile", new { iuserID = iuserID });
        }


    }


}
