using System;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CompleteDotNetCoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Upsert
        public IActionResult Upsert(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                // Create Product
                return View(company);
            }
            else
            {
                // Update Product
                company = _unitOfWork.Company.GetFirstOrDefault(
                    u => u.Id == id);
                return View(company);
            }
        }

        // Post: Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "You've successfully added a product.";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "You've successfully updated a product.";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        // Get Company List
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Company> companyList =
                _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        // Post: Delete Company
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Company obj = _unitOfWork.Company.GetFirstOrDefault(
                u => u.Id == id);

            if (obj == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Object not found."
                });
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "You've successfully deleted a company."
            });
        }

        #endregion
    }
}

