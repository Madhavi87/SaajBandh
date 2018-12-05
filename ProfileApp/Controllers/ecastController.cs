using DAL;
using ProfileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProfileApp
{
    public class ecastController : Controller
    {

        ProfileEntities db = new ProfileEntities();
        //
        // GET: /ecast/

        public ActionResult Home()
        {
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

        public ActionResult Register()
        {
            var selectlist = new SelectList(
      new List<SelectListItem>
    {
        new SelectListItem { Selected = true, Text = "Leva Patil", Value = "Leva Patil"},
          new SelectListItem { Selected = true, Text = "GujarPatil", Value = "GujarPatil"},
            new SelectListItem { Selected = true, Text = "Mali", Value = "Mali"},
              new SelectListItem { Selected = true, Text = "Marwadi", Value = "Marwadi"},
                new SelectListItem { Selected = true, Text = "Gujarathi", Value = "Gujarathi"},
                  new SelectListItem { Selected = true, Text = "Bramhan", Value = "Bramhan"},
                    new SelectListItem { Selected = true, Text = "Wani", Value = "Wani"},

                     new SelectListItem { Selected = true, Text = "Shimpi", Value = "Shimpi"},
          new SelectListItem { Selected = true, Text = "Sonar", Value = "Sonar"},
            new SelectListItem { Selected = true, Text = "Dhobi", Value = "Dhobi"},
              new SelectListItem { Selected = true, Text = "Badgujar", Value = "Badgujar"},
                new SelectListItem { Selected = true, Text = "Nhavi", Value = "Nhavi"},
                  new SelectListItem { Selected = true, Text = "Kasar", Value = "Kasar"},
                    new SelectListItem { Selected = true, Text = "Kumbhar", Value = "Kumbhar"},
      
                        new SelectListItem { Selected = true, Text = "Teli", Value = "Teli"},
                    new SelectListItem { Selected = true, Text = "Bhavsar", Value = "Bhavsar"},

                          new SelectListItem { Selected = true, Text = "Sutar", Value = "Sutar"},
                    new SelectListItem { Selected = true, Text = "Mali", Value = "Mali"},

                          new SelectListItem { Selected = true, Text = "Bari", Value = "Bari"},
                    new SelectListItem { Selected = true, Text = "Gurav", Value = "Gurav"},
      
                        new SelectListItem { Selected = true, Text = "Manbhav", Value = "Manbhav"},
                    new SelectListItem { Selected = true, Text = "Lohar", Value = "Lohar"},

                          new SelectListItem { Selected = true, Text = "Thakur", Value = "Thakur"},
                    new SelectListItem { Selected = true, Text = "Gosavi", Value = "Gosavi"},

                       new SelectListItem { Selected = true, Text = "Beldar", Value = "Beldar"},
                    new SelectListItem { Selected = true, Text = "Otaari", Value = "Otaari"},

                    
                       new SelectListItem { Selected = true, Text = "Gawali", Value = "Gawali"},
                    new SelectListItem { Selected = true, Text = "Ramoshi", Value = "Ramoshi"},

                      
                       new SelectListItem { Selected = true, Text = "Banjara", Value = "Banjara"},
                    new SelectListItem { Selected = true, Text = "Dhangar", Value = "Dhangar"},
          new SelectListItem { Selected = true, Text = "Vanjari", Value = "Vanjari"},
           new SelectListItem { Selected = true, Text = "Other Caste", Value = "OtherCaste"}
      
    }, "Value", "Text", 1);


            ViewBag.CasteList = selectlist;
            ViewBag.Genders = db.Genders.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegVM obj)
        {
            bool em = db.UserRegistrations.Any(x => x.Email == obj.Email);
            if (em == true)
            {
                ModelState.AddModelError("Email", "Email Already Used");
            }
            if (obj.Caste == "")
            {
                ModelState.AddModelError("Caste", "Please select caste");
            }
            if (!obj.TermsAndConditions)
            {
                ModelState.AddModelError("TermsAndConditions", "Please Select Terms & Conditions Box");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    OtherCasteUserRegistration o = new OtherCasteUserRegistration();
                    o.Email = obj.Email;
                    o.Mobile = obj.Mobile;
                    o.FirstName = obj.FirstName;
                    o.MiddleName = obj.MiddleName;
                    o.LastName = obj.LastName;
                    o.IsActive = false;
                    o.Gender = obj.Gender;
                    o.CasteName = obj.Caste;
                    o.SubCasteName = obj.SubCaste;
                    o.DOB = Convert.ToDateTime(obj.BirthDateDisplay);

                    o.CreatedOn = DateTime.Now;
                    db.OtherCasteUserRegistrations.Add(o);
                    db.SaveChanges();




                    return View("LandingPage");
                }
                catch (Exception)
                {

                    return RedirectToAction("Login");
                }
            }
            else
            {
                var selectlist = new SelectList(
      new List<SelectListItem>
    {
        new SelectListItem { Selected = true, Text = "Leva Patil", Value = "Leva Patil"},
          new SelectListItem { Selected = true, Text = "GujarPatil", Value = "GujarPatil"},
            new SelectListItem { Selected = true, Text = "Mali", Value = "Mali"},
              new SelectListItem { Selected = true, Text = "Marwadi", Value = "Marwadi"},
                new SelectListItem { Selected = true, Text = "Gujarathi", Value = "Gujarathi"},
                  new SelectListItem { Selected = true, Text = "Bramhan", Value = "Bramhan"},
                    new SelectListItem { Selected = true, Text = "Wani", Value = "Wani"},

                     new SelectListItem { Selected = true, Text = "Shimpi", Value = "Shimpi"},
          new SelectListItem { Selected = true, Text = "Sonar", Value = "Sonar"},
            new SelectListItem { Selected = true, Text = "Dhobi", Value = "Dhobi"},
              new SelectListItem { Selected = true, Text = "Badgujar", Value = "Badgujar"},
                new SelectListItem { Selected = true, Text = "Nhavi", Value = "Nhavi"},
                  new SelectListItem { Selected = true, Text = "Kasar", Value = "Kasar"},
                    new SelectListItem { Selected = true, Text = "Kumbhar", Value = "Kumbhar"},
      
                        new SelectListItem { Selected = true, Text = "Teli", Value = "Teli"},
                    new SelectListItem { Selected = true, Text = "Bhavsar", Value = "Bhavsar"},

                          new SelectListItem { Selected = true, Text = "Sutar", Value = "Sutar"},
                    new SelectListItem { Selected = true, Text = "Mali", Value = "Mali"},

                          new SelectListItem { Selected = true, Text = "Bari", Value = "Bari"},
                    new SelectListItem { Selected = true, Text = "Gurav", Value = "Gurav"},
      
                        new SelectListItem { Selected = true, Text = "Manbhav", Value = "Manbhav"},
                    new SelectListItem { Selected = true, Text = "Lohar", Value = "Lohar"},

                          new SelectListItem { Selected = true, Text = "Thakur", Value = "Thakur"},
                    new SelectListItem { Selected = true, Text = "Gosavi", Value = "Gosavi"},

                       new SelectListItem { Selected = true, Text = "Beldar", Value = "Beldar"},
                    new SelectListItem { Selected = true, Text = "Otaari", Value = "Otaari"},

                    
                       new SelectListItem { Selected = true, Text = "Gawali", Value = "Gawali"},
                    new SelectListItem { Selected = true, Text = "Ramoshi", Value = "Ramoshi"},

                      
                       new SelectListItem { Selected = true, Text = "Banjara", Value = "Banjara"},
                    new SelectListItem { Selected = true, Text = "Dhangar", Value = "Dhangar"},
          new SelectListItem { Selected = true, Text = "Vanjari", Value = "Vanjari"},
           new SelectListItem { Selected = true, Text = "Other Caste", Value = "OtherCaste"},
    }, "Value", "Text", 1);


                ViewBag.CasteList = selectlist;
                ViewBag.Genders = db.Genders.ToList();
                return View(obj);
            } 
        }
    }
}
