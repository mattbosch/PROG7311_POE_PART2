using POETEST_MVC.Models;
using POETEST_MVC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POETEST_MVC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int farmerID, DateTime? startDate, DateTime? endDate, string productType)
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
            ViewBag.FarmerID = farmerID;

            return View(products);
        }

        public ActionResult CreateProduct(int farmerID)
        {
            ViewBag.FarmerID = farmerID;
            return View("CreateProduct");
        }

        public ActionResult ProcessCreateProduct(Product productModel)
        {
            //checks whether all data has been entered. True: Moves to index page and creates product. False: Stays on Create Product page and notifies of validation errors.
            if (ModelState.IsValid)
            {
                SecurityDAO productDAO = new SecurityDAO();
                productDAO.createProduct(productModel);

                return RedirectToAction("Index", "Product", new { farmerID = productModel.farmerID });

            }
            else
            {
                ViewBag.FarmerID = productModel.farmerID;
                return View("CreateProduct");
            }
        }
    }
}