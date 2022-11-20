using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CompleteDotNetCoreWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objProductList =
                _unitOfWork.Product.GetAll();
            return View(objProductList);
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

            //Product product = new();
            //IEnumerable<SelectListItem> CategoryList =
            //    _unitOfWork.Category.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            //IEnumerable<SelectListItem> CoverTypeList =
            //    _unitOfWork.CoverType.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            if (id == null || id == 0)
            {
                // Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productViewModel);
            }
            else
            {

            }
            //var coverTypeFromDb = _db.Product.Find(id);
            //Product? productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            //if (productFromDb == null)
            //{
            //    return NotFound();
            //}
            return View();
        }


        // Post: Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel obj, IFormFile file)
        {
            Console.WriteLine("isValidModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                //_unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "You've successfully edited a product.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}