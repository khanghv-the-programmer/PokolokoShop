using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokolokoShop.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PokolokoShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private PokolokoShopContext _db;
        int totalRecords = 0;
        int pageSize = 20;
        int totalPage = 0;

        public ProductController(ILogger<ProductController> logger, PokolokoShopContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult CreateProduct()
        {

            List<SubCategory> subCate = _db.SubCategory.ToList();
            List<Brand> brands = _db.Brand.ToList();
            ViewBag.ProductSubcategories = subCate;
            ViewBag.Brands = brands;
            return View("CreateProduct");
        }

        
        public IActionResult AddImage(IFormCollection col)
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddProduct(IFormCollection col)
        { 
        bool isValid = true;

        string name = col["Name"];
            if(!System.Text.RegularExpressions.Regex.IsMatch(name, "[A-Za-z0-9]{5,49}"))
            {
                isValid = false;
                ViewBag.NERR = "Invalid Name";
            }
    
           
        decimal price = 0;

try
{
    price = Convert.ToInt32(col["Price"]);

}
catch (Exception)
{

    isValid = false;
    ViewBag.PERR = "Invalid Price!";
}
int quantity = 0;
try
{
    quantity  = Convert.ToInt32(col["Quantity"]);

}
catch (Exception)
{

    isValid = false;
    ViewBag.QERR = "Invalid Input!";
}
            string description = col["Description"];
            if(!System.Text.RegularExpressions.Regex.IsMatch(description, "[A-Za-z0-9]{1,199}"))
            {
                isValid = false;
                ViewBag.DERR = "Invalid Description!";
            }
            string material = col["Material"];
            if(!System.Text.RegularExpressions.Regex.IsMatch(material, "[A-Za-z0-9]{1,19}"))
            {
                isValid = false;
                ViewBag.MERR = "Invalid Material!";
            }
        int subCategoryCode = Convert.ToInt32(col["SubCategory"]);
        int brandId = Convert.ToInt32(col["Brand"]);


if (isValid)
{
    Product p = new Product
    {
        ProductName = name,    
        Price = price,
        QuantityInStock = quantity,
        Material = material,
        Description = description,
        BrandId = brandId,
        SubCategoryId = subCategoryCode,
        Status = "Available",
        DateModified = DateTime.Now

    };
    _db.Product.Add(p);
    _db.SaveChanges();
    Product newPro = _db.Product.OrderByDescending(p => p).FirstOrDefault();
                ViewBag.ID = newPro.ProductId;
                
                return AddThumbnailImg();
}
else
{
                ViewBag.Name = name;
                ViewBag.Price = price;
                ViewBag.Quantity = quantity;
                ViewBag.Material = material;
                ViewBag.Description = description;
                return CreateProduct();
}




            
        }
        public IActionResult AddThumbnailImg()
        {
            return View("AddThumbnailImg");
        }
        [HttpPost]
        public IActionResult SaveThumbnailImg(IFormCollection col)
        {
            string action = col["action"];
            string imgName = null;
            if (!action.Equals("skip"))
            {
                imgName = col["thumbimage"];
            }


            Image i = null;
            if (imgName != null)
            {
                i = new Image
                {
                    Name = imgName,
                    ThumnailImg = "/img/" + imgName,
                    LargeImage = null,
                };
            } else
            {
                i = new Image
                {
                    Name = "Default",
                    ThumnailImg = null,
                    LargeImage = null
            };
        }
            
            _db.Image.Add(i);
            _db.SaveChanges();
            int idImg = _db.Image.OrderByDescending(p => p).FirstOrDefault().Idimage;
            ProductImage pi = new ProductImage
            {
                Idimage = idImg,
                Idproduct = Convert.ToInt32(col["ID"]),

            };
            _db.ProductImage.Add(pi);
            _db.SaveChanges();

            return AddBigImg(idImg);
        }

        public IActionResult AddBigImg(int ImgId)
        {
            ViewBag.IDIMG = ImgId;
            return View("AddBigImg");
        }
        [HttpPost]
        public IActionResult SaveBigImg(IFormCollection col)
        {
            string action = col["action"];
            string imgName = null;
            if (!action.Equals("skip"))
            {
                imgName = col["bigimage"];
            }
            int idImg = Convert.ToInt32(col["ID"]);
            Image i = _db.Image.FirstOrDefault(i => i.Idimage == idImg);
            i.LargeImage = "/img/" + imgName;
            _db.Image.Update(i);
            _db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }


        public IActionResult SearchProductByCategoryAndSubCategory(string idSubString, int? getPage)
        {
            int idSub = 0;
            try
            {
                idSub = Convert.ToInt32(idSubString);
            }
            catch (Exception)
            {

                idSub = 0;
            }

            List<Product> products = _db.Product.Where(products => products.SubCategoryId == idSub).ToList();

            List<Product> currentList = new List<Product>();
            int currentPage = 0;
            totalRecords = products.Count;
            if (totalRecords % pageSize == 0)
            {
                totalPage = totalRecords / pageSize;
            }
            else
            {
                totalPage = totalRecords / pageSize + 1;
            }

            if (getPage == null)
            {
                currentPage = default(int);
            }
            else
            {
                currentPage = getPage.GetValueOrDefault();


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
                if (i >= totalRecords)
                {
                    break;
                }
                currentList.Add(products.ElementAt(i));
            }
            string nameSub = "Not found";
            nameSub = _db.SubCategory.FirstOrDefault(ProductSubcategory => ProductSubcategory.SubCategoryId == idSub).SubCateName;
            ViewBag.NameSub = nameSub;
            ViewBag.TotalPage = totalPage;
            ViewBag.CurrentPage = currentPage;
            ViewBag.IdSub = idSubString;



            return View(currentList);
        }
    }
}
