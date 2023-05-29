using POETEST_MVC.Models;
using POETEST_MVC.Services.Business;
using POETEST_MVC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POETEST_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Login(User userModel)
        {
            SecurityDAO securityDAO = new SecurityDAO();

                string role;
                bool foundUser = securityDAO.FindUser(userModel, out role);

           if (foundUser && role == "Employee")
           {
                    // Redirect to the employee home page
                    return RedirectToAction("Index", "Farmer");
           }
           else if (role == "Farmer")
           {
                    // Redirect to the farmer home page
                    return RedirectToAction("Index", "Product", new { farmerID = securityDAO.getFarmerID(securityDAO.getUserID(userModel)) });
           }
           else
           {
                // Authentication failed, display login view with error
                ModelState.AddModelError("", "Invalid username or password.");
                return View(userModel);
           }

            
        }
    }
}