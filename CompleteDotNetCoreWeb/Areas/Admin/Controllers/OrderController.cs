using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CompleteDotNetCoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: Order Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Order Details
        public IActionResult Details(int? orderId)
        {
            OrderViewModel = new()
            {
                // OrderHeader Id should match orderId passed in.
                OrderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == orderId,
                includeProperties: "ApplicationUser"),

                // Order Detail's OrderId property should match orderId passed in.
                OrderDetail = _unitOfWork.OrderDetail
                .GetAll(u => u.OrderId == orderId, includeProperties:
                "Product")
            };

            return View(OrderViewModel);
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.RoleAdmin) || User.IsInRole(SD.RoleEmployee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties:
                    "ApplicationUser");
            }
            else
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeaders = _unitOfWork.OrderHeader.GetAll(u =>
                u.ApplicationUserId == claim.Value, includeProperties:
                "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u =>
                    u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inProcess":
                    orderHeaders = orderHeaders.Where(u =>
                    u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u =>
                    u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u =>
                    u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}

