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

            return View(products);
        }

        public ActionResult CreateProduct()
        {
            return View("CreateProduct");
        }

        public ActionResult ProcessCreateProduct(Product productModel)
        {
            if (ModelState.IsValid)
            {
                SecurityDAO productDAO = new SecurityDAO();
                productDAO.createProduct(productModel);

                return RedirectToAction("Index", "Product", new { farmerID = productModel.farmerID });

            }
            else
            {
                return View("CreateProduct");
            }
        }
    }
}