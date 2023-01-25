using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompleteDotNetCoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET Index
        public IActionResult Index()
        {
            IEnumerable<CoverType> objectCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objectCoverTypeList);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // Post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CustomError", "DisplayOrder cannot match Name.");
            //}

            // ModelState is only hit when there are no client side validations.
            Console.WriteLine("isValidModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "You've successfully created a cover type.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var coverTypeFromDb = _db.Categories.Find(id);
            CoverType? coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }

        // Post: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CustomError", "DisplayOrder cannot match Name.");
            //}

            Console.WriteLine("isValidModelState: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "You've successfully edited a cover type.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            CoverType? coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }

        // Post: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(CoverType obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "You've successfully deleted a cover type.";
            return RedirectToAction("Index");
        }

        // GET /Page/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}