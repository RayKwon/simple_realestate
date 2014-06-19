using SimpleRealestate.Dao;
using SimpleRealestate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleRealestate.Controllers
{
    public class HomeController : Controller
    {
        private IPropertyDao _propertyDao;
        public HomeController(IPropertyDao propertyDao)
        {
            _propertyDao = propertyDao;
        }
        public ActionResult Index(string propertyType, string address, int? minPrice, int? maxPrice)
        {
            List<PropertyInfo> model;
            PropertyType inputPropertyType;
            if (String.IsNullOrWhiteSpace(propertyType) || propertyType.Trim().ToLower().Equals("all"))
            {
                inputPropertyType = PropertyType.All;
            }else if (!String.IsNullOrWhiteSpace(propertyType) && propertyType.Trim().ToLower().Equals("rent"))
            {
                inputPropertyType = PropertyType.Rent;
            }else if (!String.IsNullOrWhiteSpace(propertyType) && propertyType.Trim().ToLower().Equals("sale"))
            {
                inputPropertyType = PropertyType.Sale;
            }
            else
            {
                inputPropertyType = PropertyType.All;
            }
            model = _propertyDao.SearchProperties(inputPropertyType, address, minPrice, maxPrice);

            ViewBag.propertyType = propertyType;
            ViewBag.address = address;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;

            return View(model);
        }

        [ActionName("detail")]
        public ActionResult ShowDetail(int id)
        {
            var model = _propertyDao.GetPropertyById(id);
            return View(model);
        }

        public ActionResult Enquire()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("enquire")]
        public ActionResult Enquire(PropertyInfo propertyInfo)
        {
            if (ModelState.IsValid)
            {
                var result = _propertyDao.AddEnquiry(propertyInfo.Id, propertyInfo.EnquiryInput.Email, propertyInfo.EnquiryInput.Comment);
                if (result > 0)
                {
                    TempData["PropertyId"] = propertyInfo.Id;
                    return RedirectToAction("Success", "Home");
                }
                else
                {
                    ModelState.AddModelError("enquiry_fail", "Sending enquiry failed.");
                }
            }

            return View(propertyInfo);
        }

        public ActionResult Success()
        {
            ViewBag.PropertyId = TempData["PropertyId"].ToString();
            return View();
        }

    }
}