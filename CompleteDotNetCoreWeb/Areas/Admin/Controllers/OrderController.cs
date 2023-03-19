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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

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


        // POST: Order Details Pay Now
        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPayNow()
        {
            OrderViewModel.OrderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderViewModel.OrderHeader.Id,
                includeProperties: "ApplicationUser");

            OrderViewModel.OrderDetail = _unitOfWork.OrderDetail
            .GetAll(u => u.OrderId == OrderViewModel.OrderHeader.Id,
            includeProperties: "Product");

            // Stripe logic
            string domain = "https://localhost:7103/";

            //string domain = "https://completedotnetcoreweb.azurewebsites.net/";
            SessionCreateOptions options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"Admin/Order/PaymentConfirmation?OrderHeaderId={OrderViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"Admin/Order?Details?OrderId={OrderViewModel.OrderHeader.Id}"
            };

            foreach (OrderDetail item in OrderViewModel.OrderDetail)
            {

                SessionLineItemOptions sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                        }
                    },
                    Quantity = item.Count,
                };

                options.LineItems.Add(sessionLineItem);
            }

            SessionService service = new SessionService();
            Session session = service.Create(options);

            // Collect Stripe SessionId and PaymentIntentId
            _unitOfWork.OrderHeader.UpdateStripePaymentId(OrderViewModel
                .OrderHeader.Id, session.Id, session.PaymentIntentId);

            //Console.WriteLine("************ session Id: " + session.Id +
            //    "************");

            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            // End original Stripe logic

            //return View(OrderViewModel);
        }


        public IActionResult PaymentConfirmation(int OrderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == OrderHeaderId);

            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                SessionService service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(OrderHeaderId, orderHeader.SessionId,
                        session.PaymentIntentId);

                    _unitOfWork.OrderHeader.UpdateStatus(OrderHeaderId, orderHeader.OrderStatus,
                        SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(OrderHeaderId);
        }


        // POST: Update Order Details
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            OrderHeader orderHeaderFromDb = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id ==
                OrderViewModel.OrderHeader.Id, tracked: false);

            orderHeaderFromDb.Name = OrderViewModel.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
            orderHeaderFromDb.Address = OrderViewModel.OrderHeader.Address;
            orderHeaderFromDb.City = OrderViewModel.OrderHeader.City;
            orderHeaderFromDb.ZipCode = OrderViewModel.OrderHeader.ZipCode;
            if (OrderViewModel.OrderHeader.Carrier != null)
            {
                orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
            }
            if (OrderViewModel.OrderHeader.TrackingNumber != null)
            {
                orderHeaderFromDb.TrackingNumber =
                    OrderViewModel.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            //Console.WriteLine("****Order Details Updated.****");
            TempData["Success"] = "Successfully updated Order Details.";

            return RedirectToAction("Details", "Order",
                new { orderId = orderHeaderFromDb.Id });
        }


        // POST: Start Processing Order
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(
                OrderViewModel.OrderHeader.Id, SD.StatusInProcess);

            _unitOfWork.Save();
            //Console.WriteLine("****Order Status Updated.****");
            TempData["Success"] = "Successfully updated Order Status.";

            return RedirectToAction("Details", "Order",
                new { orderId = OrderViewModel.OrderHeader.Id });
        }


        // POST: Ship Order
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeaderFromDb = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id ==
                OrderViewModel.OrderHeader.Id, tracked: false);

            orderHeaderFromDb.TrackingNumber =
                OrderViewModel.OrderHeader.TrackingNumber;
            orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
            orderHeaderFromDb.OrderStatus = SD.StatusShipped;
            orderHeaderFromDb.ShippingDate = DateTime.UtcNow;
            if (orderHeaderFromDb.PaymentStatus ==
                SD.PaymentStatusDelayedPayment)
            {
                orderHeaderFromDb.PaymentDueDate = DateTime.UtcNow
                    .AddDays(30);
            }

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);

            _unitOfWork.Save();
            //Console.WriteLine("****Order Shipped.****");
            TempData["Success"] = "Successfully shipped order.";

            return RedirectToAction("Details", "Order",
                new { orderId = OrderViewModel.OrderHeader.Id });
        }


        // POST: Cancel Order
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            OrderHeader orderHeaderFromDb = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id ==
                OrderViewModel.OrderHeader.Id, tracked: false);

            if (orderHeaderFromDb.PaymentStatus == SD.PaymentStatusApproved)
            {
                RefundCreateOptions options = new()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId
                };
                RefundService service = new();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(
                    orderHeaderFromDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(
                    orderHeaderFromDb.Id, SD.StatusCancelled, SD.StatusCancelled);
            }

            _unitOfWork.Save();
            //Console.WriteLine("****Order Cancelled.****");
            TempData["Success"] = "Successfully cancelled order.";

            return RedirectToAction("Details", "Order",
                new { orderId = OrderViewModel.OrderHeader.Id });
        }


        // POST DELETE Order
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Delete()
        {
            OrderHeader order = _unitOfWork.OrderHeader.GetFirstOrDefault(
                u => u.Id == OrderViewModel.OrderHeader.Id);

            Console.WriteLine("Order: " + order.Id);

            if (order == null)
            {
                return NotFound();
            }

            _unitOfWork.OrderHeader.Remove(order);
            _unitOfWork.Save();
            TempData["Success"] = "Successfully deleted order.";
            return RedirectToAction("Index");
        }


        // POST DELETE ALL Order
        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult DeleteALL()
        {
            IEnumerable<OrderHeader> orderHeaders =
                _unitOfWork.OrderHeader.GetAll();

            Console.WriteLine("*** Order: " + orderHeaders.ToList()
                + "***");


            _unitOfWork.OrderHeader.RemoveRange(orderHeaders);
            _unitOfWork.Save();
            TempData["Success"] = "Successfully deleted all orders.";
            return RedirectToAction("Index");
        }




        // GET /Page/Error
        public IActionResult Error()
        {
            return View();
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

