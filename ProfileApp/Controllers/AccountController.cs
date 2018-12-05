using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using ProfileApp.Models;
using System.Security;
using System.Web.Security;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Security.Cryptography;


namespace ProfileApp.Controllers
{
    public class AccountController : Controller
    {

        ProfileEntities db = new ProfileEntities();

        public ActionResult Register()
        {
            //ViewBag.Country = db.Countries.ToList();
            //ViewBag.States = db.States.OrderBy(x => x.StateName).ToList();
            //ViewBag.Cities = db.Cities.Where(x => x.StateID == 0).OrderBy(x => x.CityName).ToList();
            ViewBag.Genders = db.Genders.ToList();
            return View();
        }

        public JsonResult GetCities(int id)
        {
            var data = db.Cities.Where(x => x.StateID == id).OrderBy(x => x.CityName).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTalukas(int id)
        {
            var data = db.Talukas.Where(x => x.CityID == id).OrderBy(x => x.Taluka1).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LandingPageTest()
        {
            return View(GetPaymentModelTest());
        }



        public ActionResult LandingPage()
        {
            return View(GetPaymentModel());
        }

        [HttpPost]
        public ActionResult Register(UserRegVM obj)
        {

            if (string.IsNullOrEmpty(obj.Password))
            {
                ModelState.AddModelError("Password", "Please Enter Password");
            }

            if (obj.Password != obj.ConfPassword)
            {
                ModelState.AddModelError("ConfPassword", "Password Do Not Match");
            }

            if (!obj.TermsAndConditions)
            {
                ModelState.AddModelError("TermsAndConditions", "Please Select Terms & Conditions Box");
            }

            bool em = db.UserRegistrations.Any(x => x.Email == obj.Email);
            if (em == true)
            {
                ModelState.AddModelError("Email", "Email Already Used");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    UserRegistration o = new UserRegistration();
                    o.Email = obj.Email;
                    o.Password = obj.Password;
                    o.Mobile = obj.Mobile;
                    o.FirstName = obj.FirstName;
                    o.MiddleName = obj.MiddleName;
                    o.LastName = obj.LastName;

                    o.IsActive = false;
                    o.CreatedOn = DateTime.Now;
                    db.UserRegistrations.Add(o);
                    db.SaveChanges();

                    UserProfile oUserProfile = new UserProfile();
                    oUserProfile.Gender = obj.Gender;
                    oUserProfile.UserID = o.UserID;

                    if (oUserProfile.Gender == 1)
                        oUserProfile.MyAppID = "SAAJB" + o.UserID;
                    else if (oUserProfile.Gender == 2)
                        oUserProfile.MyAppID = "SAAJG" + o.UserID;


                    db.UserProfiles.Add(oUserProfile);
                    db.SaveChanges();

                    //  var profile = db.UserProfiles.ToList().Where(ite => ite.UserID == oUserProfile.UserID).FirstOrDefault();
                    Session["MyAppId"] = oUserProfile.MyAppID;
                    Session["UserID"] = oUserProfile.UserID;
                    Session["Email"] = o.Email;
                    Session["Phone"] = o.Mobile;
                    Session["FirstName"] = o.FirstName;
                    Session["MyAppId"] = oUserProfile.MyAppID;
                    //  SendMail(o.Email);

                    return RedirectToAction("LandingPage");
                }
                catch (Exception)
                {

                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.Genders = db.Genders.ToList();
                //ViewBag.Country = db.Countries.ToList();
                //ViewBag.States = db.States.OrderBy(x => x.StateName).ToList();
                //ViewBag.Cities = db.Cities.Where(x => x.StateID == 0).OrderBy(x => x.CityName).ToList();
                return View(obj);
            }

        }



        public ActionResult Login()
        {


            return View();
        }


        [HttpPost]
        public ActionResult Login(UserRegVM obj)
        {
            var userdetails = (from context in db.UserRegistrations
                               join sub in db.UserProfiles on context.UserID equals sub.UserID
                               where sub.MyAppID.ToUpper() == obj.MyAppId.ToUpper() &&
                               context.Password == obj.Password
                               select new
                               {
                                   Email = context.Email,
                                   IsActive = context.IsActive,
                                   MyappId = sub.MyAppID,
                                   UserID = sub.UserID,
                                   Name = context.FirstName + context.LastName,
                                   Phone = context.Mobile

                               }).FirstOrDefault();




            if (userdetails != null && userdetails.IsActive == true)
            {
                FormsAuthentication.SetAuthCookie(userdetails.MyappId, true);
                Session["IsAuthenticated"] = true;
                Session["UserID"] = userdetails.UserID;
                Session["Email"] = userdetails.Email;
                Session["Phone"] = userdetails.Phone;
                Session["Role"] = "User";
                Session["FirstName"] = userdetails.Name;

                return RedirectToAction("ViewProfile", "Profile");
            }
            else if (userdetails != null && userdetails.IsActive == false)
            {
                //     ModelState.AddModelError("ErrMsg", "Your Account is Not Activated Yet. Please Contact Support Team.");
                //    return View(obj);

                Session["MyAppId"] = userdetails.MyappId;
                Session["UserID"] = userdetails.UserID;
                Session["Email"] = userdetails.Email;
                Session["Phone"] = userdetails.Phone;
                Session["Role"] = "User";
                Session["FirstName"] = userdetails.Name;
                return RedirectToAction("LandingPage");
            }
            else
            {
                ModelState.AddModelError("ErrMsg", "Invalid Credentials");
                return View(obj);
            }

        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["Email"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public void SendMail(string Rec)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("Welcome User,<br>");
            sb.Append("We Have Your Registration.<br>");
            sb.Append("To Activate Your Account, Please Contact to Admin<br>");
            sb.Append("Admin Mobile : 9403378624");

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("kapilpatil143@gmail.com");
            msg.To.Add(Rec);
            msg.Subject = "Account Activation..!";
            msg.Body = sb.ToString();
            msg.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient();
            sc.Host = "smtp.gmail.com";
            sc.Port = 587;
            sc.Credentials = new System.Net.NetworkCredential("kapilpatil143@gmail.com", "kapil143");
            sc.EnableSsl = true;
            msg.To.Add(Rec);
            msg.Subject = "Account Activation..!";
            msg.Body = sb.ToString();
            msg.IsBodyHtml = true;
            try
            {
                sc.Send(msg);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public ActionResult FeedBack()
        {
            FeedBackModel iFeedBackModel = new FeedBackModel();
            return View(iFeedBackModel);
        }

        [HttpPost]
        public ActionResult FeedBack(FeedBackModel iFeedBackModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Feedback objFeedback = new Feedback();
                    objFeedback.MobileNo = iFeedBackModel.Mobile;
                    objFeedback.Email = iFeedBackModel.Email;
                    objFeedback.Name = iFeedBackModel.Name;
                    objFeedback.Comment = iFeedBackModel.Comments;
                    objFeedback.CreatedOn = DateTime.Now;
                    db.Feedbacks.Add(objFeedback);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
            ModelState.Clear();
            ViewBag.SavedData = true;
            return View("FeedBack", new FeedBackModel());
        }


        public PaymentModel GetPaymentModel()
        {
            PaymentModel objPaymentModel = new PaymentModel();

            objPaymentModel.action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";
            objPaymentModel.amount = "500";
            objPaymentModel.key = ConfigurationManager.AppSettings["MERCHANT_KEY"];

            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            objPaymentModel.txnid = strHash.ToString().Substring(0, 20); ;
            objPaymentModel.productinfo = "Saajbandh";
            objPaymentModel.firstname = Convert.ToString(Session["FirstName"]);
            objPaymentModel.phone = Convert.ToString(Session["FirstName"]);
            objPaymentModel.email = Convert.ToString(Session["Email"]);
            string iuserid = Base64Encode(Convert.ToString(Session["Email"]) + "cryptho");
            objPaymentModel.surl = "http://saajbandh.com/account/PaymentCompleted?key=" + iuserid;
            objPaymentModel.furl = "http://saajbandh.com/account/PaymentFailed";
            objPaymentModel.service_provider = "payu_paisa";


            string hash1 = "";
            string[] hashVarsSeq;
            string hash_string = "";
            hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
            hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + ConfigurationManager.AppSettings["MERCHANT_KEY"];
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + objPaymentModel.txnid;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + objPaymentModel.amount;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hash_string = hash_string + objPaymentModel.productinfo;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "firstname")
                {
                    hash_string = hash_string + objPaymentModel.firstname;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "email")
                {
                    hash_string = hash_string + objPaymentModel.email;
                    hash_string = hash_string + '|';
                }
                else
                {

                    hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                    hash_string = hash_string + '|';
                }
            }
            hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

            //hash_string = "skZY06H1|8a8f6135ddec9b83eab0|1|tes|Saajbandh|chetansharma331@gmail.com|||||||||||ktmr2GPQvM";
            hash1 = Generatehash512(hash_string).ToLower();         //generating hash
            objPaymentModel.hash = hash1;
            return objPaymentModel;


        }

        public PaymentModel GetPaymentModelTest()
        {
            PaymentModel objPaymentModel = new PaymentModel();

            objPaymentModel.action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";
            objPaymentModel.amount = "1";
            objPaymentModel.key = ConfigurationManager.AppSettings["MERCHANT_KEY"];

            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            objPaymentModel.txnid = strHash.ToString().Substring(0, 20); 
            objPaymentModel.productinfo = "Saajbandh";
            objPaymentModel.firstname = "vishal";
            objPaymentModel.phone = "9960853331";
            objPaymentModel.email = "vishal.infogalaxy@gmail.com";
            string iuserid = Base64Encode("1cryptho");
            objPaymentModel.surl = "http://saajbandh.com/account/PaymentCompleted?key=" + iuserid;
            objPaymentModel.furl = "http://saajbandh.com/account/PaymentFailed";
            objPaymentModel.service_provider = "payu_paisa";


            string hash1 = "";
            string[] hashVarsSeq;
            string hash_string = "";
            hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
            hash_string = "";
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + ConfigurationManager.AppSettings["MERCHANT_KEY"];
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + objPaymentModel.txnid;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + objPaymentModel.amount;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hash_string = hash_string + objPaymentModel.productinfo;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "firstname")
                {
                    hash_string = hash_string + objPaymentModel.firstname;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "email")
                {
                    hash_string = hash_string + objPaymentModel.email;
                    hash_string = hash_string + '|';
                }
                else
                {

                    hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                    hash_string = hash_string + '|';
                }
            }
            hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

            //hash_string = "skZY06H1|8a8f6135ddec9b83eab0|1|tes|Saajbandh|chetansharma331@gmail.com|||||||||||ktmr2GPQvM";
            hash1 = Generatehash512(hash_string).ToLower();         //generating hash
            objPaymentModel.hash = hash1;
            return objPaymentModel;


        }
        public string Generatehash512(string text)
        {

            //byte[] data = new byte[200];
            //byte[] result;
            //SHA512 shaM = new SHA512Managed();
            //result = shaM.ComputeHash(data);


            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512 hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public ActionResult PaymentCompleted(string key)
        {

            string iuserid = Base64Decode(key);
            iuserid = iuserid.Replace("cryptho", "");
            UserRegistration a = db.UserRegistrations.Find(key);
            a.IsActive = true;
            db.Entry(a).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return View();

        }

        public ActionResult PaymentFailed()
        {
            return View();

        }

    }
}
public class PaymentModel
{
    public string action1 { get; set; }
    public string key { get; set; }
    public string hash { get; set; }
    public string txnid { get; set; }

    public string amount { get; set; }
    public string firstname { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string productinfo { get; set; }
    public string surl { get; set; }
    public string furl { get; set; }
    public string service_provider { get; set; }

}