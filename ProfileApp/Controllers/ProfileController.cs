using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProfileApp.Models;
using DAL;
using System.IO;

namespace ProfileApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        ProfileEntities db = new ProfileEntities();

        [MyAuth(Roles = "User")]
        public ActionResult EditProfile()
        {
            int Userid = Convert.ToInt32(Session["UserID"].ToString());

            var udata = db.UserRegistrations.Where(x => x.UserID == Userid).FirstOrDefault();
            var d = db.UserProfiles.Where(x => x.UserID == Userid).FirstOrDefault();

            UserProfileVM obj = new UserProfileVM();
            obj.UserID = d.UserID;
            obj.FirstName = udata.FirstName;
            obj.MiddleName = udata.MiddleName;
            obj.LastName = udata.LastName;

            obj.ImagePath = d.ImagePath;
            if (d.DateOfBirth == null)
            {
                obj.DateOfBirth = DateTime.Now;
            }
            else
            {
                obj.DateOfBirth = d.DateOfBirth.Value;
            }
            obj.Gender = d.Gender.Value;
            obj.Email = udata.Email;
            obj.RAddress = d.RAddress;
            obj.DateOfBirth = d.DateOfBirth.HasValue ? d.DateOfBirth.Value : DateTime.MinValue;
            obj.BirthDateDisplay = d.DateOfBirth.HasValue ? d.DateOfBirth.Value.ToString("dd MMM yyyy") : "";
            obj.BirthName = d.BirthName;
            obj.BirthPlace = d.BirthPlace;
            obj.BirthTime = d.BirthTime;
            // obj.BirthTime = d.BirthTime.HasValue ? d.BirthTime.Value : TimeSpan.MinValue;
            obj.Gotra = d.Gotra;
            obj.MulGaon = d.MulGaon;
            obj.EducationDetail = d.EducationDetail;
            obj.ServiceDetail = d.ServiceDetail;
            obj.SalaryAnual = d.SalaryAnual;

            obj.ImagePath = d.ImagePath;

            obj.FatherName = d.FatherName;
            obj.MotherName = d.MotherName;
            obj.FatherOccupation = d.FatherOccupation;
            obj.FatherContactNo = d.ContactNo;
            obj.MamaPlace = d.MamaPlace;
            obj.MamaSurName = d.MamaName;


            obj.Mobile = udata.Mobile;
            obj.MyAppID = d.MyAppID;
            obj.BloodGroupID = d.BloodGroupID.HasValue ? d.BloodGroupID.Value : 0;
            obj.ComplexionID = d.ComplexionID.HasValue ? d.ComplexionID.Value : 0;
            obj.StateID = d.StateID.HasValue ? d.StateID.Value : 0;
            obj.CityID = d.DistrictID.HasValue ? d.DistrictID.Value : 0;
            obj.MaritalStatusID = d.MritalStatus.HasValue ? d.MritalStatus.Value : 0;
            obj.HeightID = d.HeightID.HasValue ? d.HeightID.Value : 0;
            obj.TalukaID = d.TalukaID.HasValue ? d.TalukaID.Value : 0;
            obj.WeightID = d.WeightID.HasValue ? d.WeightID.Value : 0;
            obj.SubCasteID = d.SubCasteID.HasValue ? d.SubCasteID.Value : 0;
            obj.PhysicalChalengeID = d.PhysicallyChalengeID.HasValue ? d.PhysicallyChalengeID.Value : 0;

            obj.SpectacleID = d.SpectacleID.HasValue ? d.SpectacleID.Value : 0;
            obj.SchoolID = d.DegreeID.HasValue ? d.DegreeID.Value : 0;
            obj.ServiceBusinessID = d.CareerID.HasValue ? d.CareerID.Value : 0;

            ViewBag.DegreeList = db.EduDegrees.ToList();
            ViewBag.Genders = db.Genders.ToList();
            ViewBag.MaritalStatus = db.MaritalStatus.ToList();
            ViewBag.SubCastes = db.SubCastes.ToList();
            ViewBag.ComplexionList = db.Complexions.ToList();
            ViewBag.HeightList = db.Heights.ToList();
            ViewBag.BloodGroup = db.BloodGroups.ToList();
            ViewBag.GenericList = GetGenericList();
            ViewBag.CarrerTypeList = GetCarrerTypeList();

            ViewBag.States = db.States.OrderBy(x => x.StateName).ToList();
            ViewBag.Cities = db.Cities.Where(x => x.StateID == udata.StateID.Value).OrderBy(x => x.CityName).ToList();
            ViewBag.Talukas = db.Talukas.Where(x => x.CityID == udata.CityID).OrderBy(x => x.Taluka1).ToList();
            ViewBag.WeightList = db.Weights.ToList();
            return View("EditProfile", obj);



        }


        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(UserProfileVM obj)
        {
            try
            {
                UserRegistration u = db.UserRegistrations.Find(obj.UserID);
                if (u != null)
                {
                    u.FirstName = obj.FirstName;
                    u.MiddleName = obj.MiddleName;
                    u.LastName = obj.LastName;

                    u.Email = obj.Email;


                }
                UserProfile o = db.UserProfiles.Find(obj.UserID);
                o.MritalStatus = obj.MaritalStatusID;
                o.DateOfBirth = Convert.ToDateTime(obj.BirthDateDisplay); //obj.DateOfBirth;
                o.BirthName = obj.BirthName;
                o.BirthPlace = obj.BirthPlace;
                o.BirthTime = obj.BirthTime;
                o.Gotra = obj.Gotra;
                o.SubCasteID = obj.SubCasteID;
                o.ComplexionID = obj.ComplexionID;
                o.HeightID = obj.HeightID;
                o.WeightID = obj.WeightID;
                o.PhysicallyChalengeID = obj.PhysicalChalengeID;
                o.BloodGroupID = obj.BloodGroupID;
                o.SpectacleID = obj.SpectacleID;
                o.RAddress = obj.RAddress;
                o.MulGaon = obj.MulGaon;
                o.StateID = obj.StateID;
                o.DistrictID = obj.CityID;
                o.TalukaID = obj.TalukaID;

                o.DegreeID = obj.SchoolID;
                o.EducationDetail = obj.EducationDetail;
                o.CareerID = obj.ServiceBusinessID;
                o.ServiceDetail = obj.ServiceDetail;
                o.SalaryAnual = obj.SalaryAnual;

                o.FatherName = obj.FatherName;
                o.MotherName = obj.MotherName;
                o.FatherOccupation = obj.FatherOccupation;
                o.ContactNo = obj.FatherContactNo;
                o.MamaName = obj.MamaSurName;
                o.MamaPlace = obj.MamaPlace;

                HttpPostedFileBase b = Request.Files["ImageFile"] as HttpPostedFileBase;
                if (b != null)
                {
                    string pic = System.IO.Path.GetFileName(b.FileName);
                    if (pic != "")
                    {
                        string path = Path.Combine(Server.MapPath("~/Images"),
                                              Session["UserID"].ToString() + ".jpg");
                        string ipath = "/Images/" + Session["UserID"].ToString() + ".jpg";

                        b.SaveAs(path);

                        int uid = Convert.ToInt32(Session["UserID"].ToString());

                        o.ImagePath = ipath;
                    }
                }


                db.Entry(o).State = System.Data.EntityState.Modified;
                db.SaveChanges();

                db.Entry(u).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("ViewProfile");
        }



        [MyAuth(Roles = "User")]
        public ActionResult ViewProfile(int iuserID = 0)
        {
            if (iuserID == 0)
                iuserID = Convert.ToInt32(Session["UserID"].ToString());

            var udata = db.UserRegistrations.Where(x => x.UserID == iuserID).FirstOrDefault();
            var d = db.UserProfiles.Where(x => x.UserID == iuserID).FirstOrDefault();

            UserProfileVM obj = new UserProfileVM();
            obj.UserID = d.UserID;
            obj.FirstName = udata.FirstName;
            obj.MiddleName = udata.MiddleName;
            obj.LastName = udata.LastName;

            obj.ImagePath = d.ImagePath;
            if (d.DateOfBirth == null)
            {
                obj.DateOfBirth = DateTime.Now;
            }
            else
            {
                obj.DateOfBirth = d.DateOfBirth.Value;
            }
            obj.Gender = d.Gender.Value;
            obj.Email = udata.Email;
            obj.RAddress = d.RAddress;
            obj.BirthDateDisplay = d.DateOfBirth.HasValue ? d.DateOfBirth.Value.ToString("dd MMM yyyy") : "";
            obj.BirthName = d.BirthName;
            obj.BirthPlace = d.BirthPlace;
            obj.BirthTime = d.BirthTime;
            // obj.BirthTime = d.BirthTime.HasValue ? d.BirthTime.Value : TimeSpan.MinValue;
            obj.Gotra = d.Gotra;
            obj.MulGaon = d.MulGaon;
            obj.EducationDetail = d.EducationDetail;
            obj.ServiceDetail = d.ServiceDetail;
            obj.SalaryAnual = d.SalaryAnual;

            obj.ImagePath = d.ImagePath;

            obj.FatherName = d.FatherName;
            obj.MotherName = d.MotherName;
            obj.FatherOccupation = d.FatherOccupation;
            obj.FatherContactNo = d.ContactNo;
            obj.MamaPlace = d.MamaPlace;
            obj.MamaSurName = d.MamaName;


            obj.Mobile = udata.Mobile;
            obj.MyAppID = d.MyAppID;
            obj.BloodGroupID = d.BloodGroupID.HasValue ? d.BloodGroupID.Value : 0;
            obj.StateID = d.StateID.HasValue ? d.StateID.Value : 0;
            obj.CityID = d.DistrictID.HasValue ? d.DistrictID.Value : 0;
            obj.MaritalStatusID = d.MritalStatus.HasValue ? d.MritalStatus.Value : 0;
            obj.HeightID = d.HeightID.HasValue ? d.HeightID.Value : 0;
            obj.TalukaID = d.TalukaID.HasValue ? d.TalukaID.Value : 0;
            obj.WeightID = d.WeightID.HasValue ? d.WeightID.Value : 0;
            obj.SubCasteID = d.SubCasteID.HasValue ? d.SubCasteID.Value : 0;
            obj.PhysicalChalengeID = d.PhysicallyChalengeID.HasValue ? d.PhysicallyChalengeID.Value : 0;

            obj.SpectacleID = d.SpectacleID.HasValue ? d.SpectacleID.Value : 0;
            obj.SchoolID = d.DegreeID.HasValue ? d.DegreeID.Value : 0;
            obj.ServiceBusinessID = d.CareerID.HasValue ? d.CareerID.Value : 0;

            obj.DegreeName = db.EduDegrees.ToList().Where(ite => ite.EducationDegreeID == d.DegreeID).FirstOrDefault() == null ? "" : db.EduDegrees.ToList().Where(ite => ite.EducationDegreeID == d.DegreeID).FirstOrDefault().DegreeName;
            obj.GenderName = db.Genders.ToList().Where(ite => ite.GenderID == d.Gender).FirstOrDefault() == null ? "" : db.Genders.ToList().Where(ite => ite.GenderID == d.Gender).FirstOrDefault().GenderName;
            obj.MaritalStatusName = db.MaritalStatus.ToList().Where(ite => ite.StatusID == d.MritalStatus).FirstOrDefault() == null ? "" : db.MaritalStatus.ToList().Where(ite => ite.StatusID == d.MritalStatus).FirstOrDefault().Status;
            obj.SubCasteName = db.SubCastes.ToList().Where(ite => ite.SubCasteID == d.SubCasteID).FirstOrDefault() == null ? "" : db.SubCastes.ToList().Where(ite => ite.SubCasteID == d.SubCasteID).FirstOrDefault().SubCasteName;
            obj.ComplexionName = db.Complexions.ToList().Where(ite => ite.ComplexionID == d.ComplexionID).FirstOrDefault() == null ? "" : db.Complexions.ToList().Where(ite => ite.ComplexionID == d.ComplexionID).FirstOrDefault().Complexion1;
            obj.HeightName = db.Heights.ToList().Where(ite => ite.HeightID == d.HeightID).FirstOrDefault() == null ? "" : db.Heights.ToList().Where(ite => ite.HeightID == d.HeightID).FirstOrDefault().Height1;

            obj.BloodGroupName = db.BloodGroups.ToList().Where(ite => ite.BloodGroupID == d.BloodGroupID).FirstOrDefault() == null ? "" : db.BloodGroups.ToList().Where(ite => ite.BloodGroupID == d.BloodGroupID).FirstOrDefault().BloodG;

            obj.PhysicalyChalengedName = GetGenericList().Where(ite => ite.ID == d.PhysicallyChalengeID).FirstOrDefault() == null ? "" : GetGenericList().Where(ite => ite.ID == d.PhysicallyChalengeID).FirstOrDefault().Value;
            obj.SpectacleName = GetGenericList().Where(ite => ite.ID == d.SpectacleID).FirstOrDefault() == null ? "" : GetGenericList().Where(ite => ite.ID == d.SpectacleID).FirstOrDefault().Value;

            obj.SpectacleName = GetGenericList().Where(ite => ite.ID == d.SpectacleID).FirstOrDefault() == null ? "" : GetGenericList().Where(ite => ite.ID == d.SpectacleID).FirstOrDefault().Value;
            obj.CarrerTypeName = GetCarrerTypeList().Where(ite => ite.ID == d.CareerID).FirstOrDefault() == null ? "" : GetCarrerTypeList().Where(ite => ite.ID == d.CareerID).FirstOrDefault().Value;

            obj.StateName = db.States.ToList().Where(ite => ite.StateID == d.StateID).FirstOrDefault() == null ? "" : db.States.ToList().Where(ite => ite.StateID == d.StateID).FirstOrDefault().StateName;
            obj.CitiName = db.Cities.ToList().Where(ite => ite.CityID == d.DistrictID).FirstOrDefault() == null ? "" : db.Cities.ToList().Where(ite => ite.CityID == d.DistrictID).FirstOrDefault().CityName;
            obj.TalukaName = db.Talukas.ToList().Where(ite => ite.TalukaID == d.TalukaID).FirstOrDefault() == null ? "" : db.Talukas.ToList().Where(ite => ite.TalukaID == d.TalukaID).FirstOrDefault().Taluka1;
            obj.WeightName = db.Weights.ToList().Where(ite => ite.WeightID == d.WeightID).FirstOrDefault() == null ? "" : db.Weights.ToList().Where(ite => ite.WeightID == d.WeightID).FirstOrDefault().Weight1;
            return View("ViewProfile", obj);


        }

        public List<GenericList> GetGenericList()
        {
            List<GenericList> objlist = new List<GenericList>();
            objlist.Add(new GenericList { ID = 1, Value = "Yes" });
            objlist.Add(new GenericList { ID = 2, Value = "No" });
            return objlist;
        }

        public List<GenericList> GetCarrerTypeList()
        {
            List<GenericList> objlist = new List<GenericList>();
            objlist.Add(new GenericList { ID = 1, Value = "Service" });
            objlist.Add(new GenericList { ID = 2, Value = "Business" });
            return objlist;
        }



        [MyAuth(Roles = "User")]
        public ActionResult ProfileInfo()
        {
            var data = db.Get_Profile(Convert.ToInt32(Session["UserID"].ToString())).FirstOrDefault();
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

        //public ActionResult UploadPhoto()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult UploadPhoto(Photo obj)
        //{
        //    HttpPostedFileBase b = Request.Files["ImageFile"] as HttpPostedFileBase;
        //    string pic = System.IO.Path.GetFileName(b.FileName);
        //    string path = Path.Combine(Server.MapPath("~/Images"),
        //                               Session["UserID"].ToString() + ".jpg");
        //    string ipath = "/Images/" + Session["UserID"].ToString() + ".jpg";

        //    b.SaveAs(path);

        //    int uid = Convert.ToInt32(Session["UserID"].ToString());
        //    UserProfile data = db.UserProfiles.Find(uid);
        //    data.ImagePath = ipath;

        //    db.Entry(data).State = System.Data.EntityState.Modified;
        //    db.SaveChanges();

        //    return RedirectToAction("ViewProfile");


        // }
    }
}

public class GenericList
{
    public int ID { get; set; }
    public string Value { get; set; }
}

public class BusinessTypeList
{
    public int ID { get; set; }
    public string Value { get; set; }
}