using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokolokoShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PokolokoShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        int totalRecords = 0;
        int totalRecordsProducts = 0;
        int pageSize = 20;
        int totalPage = 0;
        private PokolokoShopContext _db;
        public HomeController(ILogger<HomeController> logger, PokolokoShopContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index(int? productPage)
        {
            List<Product> productList = _db.Product.ToList();
            List<Product> currentProList = new List<Product>();
            List<Category> categories = _db.Category.ToList();
            HttpContext.Session.SetString("Cate", JsonConvert.SerializeObject(categories));
            int currentPage = 0;
            totalRecordsProducts = productList.Count;
            if (totalRecordsProducts % pageSize == 0)
            {
                totalPage = totalRecordsProducts / pageSize;
            }
            else
            {
                totalPage = totalRecordsProducts / pageSize + 1;
            }
            if (productPage == null)
            {
                productPage = default(int);
            }
            else
            {
                currentPage = productPage.GetValueOrDefault();


            }
            if (currentPage > totalPage)
                currentPage = 0;
            try
            {
                Convert.ToInt32(currentPage);
            }
            catch (Exception)
            {

                currentPage = 0;
            }


            for (int i = currentPage * pageSize; i < currentPage * pageSize + pageSize; i++)
            {
                if (i >= totalRecordsProducts)
                {
                    break;
                }
                currentProList.Add(productList.ElementAt(i));
            }

            ViewBag.TotalPage = totalPage;
            ViewBag.CurrentPage = currentPage;

            return View(currentProList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductDetail(int? pId)
        {
            var productdetail = _db.Product.FirstOrDefault(pro => pro.ProductId == pId);
            return View(productdetail);
        }
    }
}
