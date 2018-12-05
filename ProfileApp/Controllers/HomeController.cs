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
using System.Web.Script.Serialization;

namespace ProfileApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/


        public ActionResult Home()
        {
            ProfileEntities db = new ProfileEntities();
            ViewBag.Genders = db.Genders.ToList();
            ViewBag.Status = db.MaritalStatus.ToList();
            var citylist = db.Cities.Where(x => x.StateID == 15).OrderBy(x => x.CityName).ToList();
            ViewBag.Cities = citylist;
            return View();
        }

        public ActionResult MainHome()
        {
            try
            {
                string ipAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                }
                LocationModel location = new LocationModel();
                string url = string.Format("http://freegeoip.net/json/{0}", ipAddress);
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url);
                    location = new JavaScriptSerializer().Deserialize<LocationModel>(json);
                }
                SaveVisitorDetails(location);

            }
            catch (Exception ex)
            {


            }
            return View();
        }

        public ActionResult AboutUS()
        {
            return View();
        }

        public ActionResult TermsAndCondition()
        {
            return View("TermsAndCondition1");
        }


        public ActionResult ContactUS()
        {
            return View();
        }

        public ActionResult UnderConstruction()
        {
            return View();
        }

        public void SaveVisitorDetails(LocationModel iLocation)
        {
            try
            {
                ProfileEntities db = new ProfileEntities();
                VisitorDeail o = new VisitorDeail();
                o = (from context in db.VisitorDeails
                     where context.IP.ToLower() == iLocation.IP.ToLower()
                     select context).FirstOrDefault();
                if (o == null)
                {
                    o = new VisitorDeail();
                    o.IP = iLocation.IP;
                    o.Country_Code = iLocation.Country_Code;
                    o.Country_Name = iLocation.Country_Name;
                    o.Region_Code = iLocation.Region_Code;
                    o.Region_Name = iLocation.Region_Name;
                    o.City = iLocation.City;
                    o.Zip_Code = iLocation.Zip_Code;
                    o.Time_Zone = iLocation.Time_Zone;
                    o.Latitude = iLocation.Latitude;
                    o.Longitude = iLocation.Longitude;
                    o.Metro_Code = iLocation.Metro_Code;
                    o.CreatedON = DateTime.Now;
                    o.AccessCount = 1;
                    o.ModifiedON = DateTime.Now;
                    db.VisitorDeails.Add(o);
                    db.SaveChanges();
                }
                else
                {
                    o.AccessCount = o.AccessCount + 1;
                    o.ModifiedON = DateTime.Now;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }

        }


    }
}
public class LocationModel
{
    public string IP { get; set; }
    public string Country_Code { get; set; }
    public string Country_Name { get; set; }
    public string Region_Code { get; set; }
    public string Region_Name { get; set; }
    public string City { get; set; }
    public string Zip_Code { get; set; }
    public string Time_Zone { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Metro_Code { get; set; }
}