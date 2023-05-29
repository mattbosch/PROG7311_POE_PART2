using POETEST_MVC.Models;
using POETEST_MVC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POETEST_MVC.Controllers
{
    public class FarmerController : Controller
    {
        // GET: Farmer
        public ActionResult Index()
        {

            List<Farmer> farmers = new List<Farmer>();

            SecurityDAO farmerDAO = new SecurityDAO();

            farmers = farmerDAO.fetchFarmers();

            return View("Index", farmers);
        }

        public ActionResult ViewProducts(int farmerID, DateTime? startDate, DateTime? endDate, string productType)
        {
            List<Product> products = new List<Product>();
            SecurityDAO productsDAO = new SecurityDAO();
            if (startDate != null && endDate != null)
            {
                // Filter products within the specified date range
                products = productsDAO.fetchProductsDateFilter(farmerID, startDate.Value, endDate.Value);
            }
            else
            {
                // Fetch all products
                products = productsDAO.fetchProducts(farmerID);
            }

            List<string> productTypes = products.Select(p => p.type).Distinct().ToList();

            if (!string.IsNullOrEmpty(productType))
            {
                // Filter products by the selected product type
                products = products.Where(p => p.type == productType).ToList();
            }

            ViewBag.ProductTypes = new SelectList(productTypes);
            ViewBag.SelectedProductType = productType;

            return View(products);
        }

        public ActionResult CreateFarmer()
        {
            return View("CreateFarmer");
        }

        public ActionResult ProcessCreateFarmer(Farmer Farmer)
        {
            if (ModelState.IsValid)
            {
                SecurityDAO farmerDAO = new SecurityDAO();
                farmerDAO.createFarmer(Farmer);

                return RedirectToAction("Index", "Farmer");

            }
            else
            {
                return View("CreateFarmer");
            }
        }

    }
}