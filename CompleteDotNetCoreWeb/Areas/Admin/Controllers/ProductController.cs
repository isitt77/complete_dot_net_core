using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompleteDotNetCoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            //IEnumerable<Product> objProductList =
            //    _unitOfWork.Product.GetAll();
            //return View(objProductList);
            return View();
        }


        // GET: Upsert
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                // Create Product
                return View(productViewModel);
            }
            else
            {
                // Update Product
                productViewModel.Product = _unitOfWork.Product
                    .GetFirstOrDefault(u => u.Id == id);
                return View(productViewModel);
            }
        }


        // Post: Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel obj, IFormFile? file)
        {
            Console.WriteLine("isValidModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string uploads = Path.Combine(wwwRootPath,
                        @"images/products");
                    string extension = Path.GetExtension(file.FileName);

                    if (obj.Product.IamgeUrl != null)
                    {
                        string oldImagePath = Path.Combine(wwwRootPath,
                            obj.Product.IamgeUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            Console.WriteLine("Replacing image: " +
                                oldImagePath);
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (FileStream fileStreams = new FileStream(
                        Path.Combine(uploads, fileName + extension),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.IamgeUrl = @"/images/products/" +
                        fileName + extension;
                    //Console.WriteLine(
                    //        "fileName: " + fileName + "\n" +
                    //        "uploads: " + uploads + "\n" +
                    //        "extension: " + extension
                    //    );
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "You've successfully added a product.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Get: Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            Product? ProductFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        // Post: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Product obj = _unitOfWork.Product.GetFirstOrDefault(
                u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            string imagePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath,
                            obj.IamgeUrl.TrimStart('/'));

            if (System.IO.File.Exists(imagePathToDelete))
            {
                Console.WriteLine("Deleting image: " + imagePathToDelete);
                System.IO.File.Delete(imagePathToDelete);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "You've successfully deleted product.";
            return RedirectToAction("Index");
        }


        // GET /Page/Error
        public IActionResult Error()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> productList =
                _unitOfWork.Product.GetAll(includeProperties:
                "Category,CoverType");
            return Json(new { data = productList });
        }


        #endregion
    }
}