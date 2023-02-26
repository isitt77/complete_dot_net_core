using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using CompleteDotNetCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace CompleteDotNetCoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public int OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: Cart Index
        public IActionResult Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == claim.Value,
                    includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (ShoppingCart cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count,
                    cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal +=
                            (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
        }

        // GET: Summary View
        public IActionResult Summary()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel = new ShoppingCartViewModel()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == claim.Value,
                    includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork
                .ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.Address = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.Address;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.ZipCode = ShoppingCartViewModel
                .OrderHeader.ApplicationUser.ZipCode;

            foreach (ShoppingCart cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count,
                    cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal +=
                    (cart.Price * cart.Count);
            }
            return View(ShoppingCartViewModel);
        }


        // Post: Order (from Summary View)
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartViewModel.CartList = _unitOfWork.ShoppingCart.GetAll(
                    u => u.ApplicationUserId == claim.Value,
                    includeProperties: "Product");


            ShoppingCartViewModel.OrderHeader.OrderDate =
                System.DateTime.UtcNow;
            ShoppingCartViewModel.OrderHeader.ApplicationUserId =
                claim.Value;

            foreach (ShoppingCart cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count,
                    cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);

                ShoppingCartViewModel.OrderHeader.OrderTotal +=
                    (cart.Price * cart.Count);
            }

            // Company User logic
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(u => u.Id == claim.Value);
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus =
                    SD.PaymentStatusPending;
                ShoppingCartViewModel.OrderHeader.OrderStatus =
                    SD.StatusPending;
            }
            else
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus =
                    SD.PaymentStatusDelayedPayment;
                ShoppingCartViewModel.OrderHeader.OrderStatus =
                    SD.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);
            _unitOfWork.Save();

            foreach (ShoppingCart cart in ShoppingCartViewModel.CartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }


            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
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
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
                    CancelUrl = domain + $"Customer/Cart/Index"
                };

                foreach (ShoppingCart item in ShoppingCartViewModel.CartList)
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
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartViewModel
                    .OrderHeader.Id, session.Id, session.PaymentIntentId);

                //Console.WriteLine("************ session Id: " + session.Id +
                //    "************");

                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

                // End original Stripe logic
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Cart",
                    new { id = ShoppingCartViewModel.OrderHeader.Id });
            }
            // End new Stripe logic
        }


        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(u => u.Id == id);

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                SessionService service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    //orderHeader.PaymentIntentId = session.PaymentIntentId;
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, orderHeader.SessionId,
                        session.PaymentIntentId);

                    //Console.WriteLine("************ PaymentIntentId: " +
                    //    session.PaymentIntentId + "************");

                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved,
                        SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == orderHeader
                .ApplicationUserId).ToList();

            HttpContext.Session.Clear();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }


        public IActionResult AddItem(int cartId)
        {
            ShoppingCart? cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult SubtractItem(int cartId)
        {
            ShoppingCart? cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);

                // Decrement session cart item count
                Int32 cartItemCount = _unitOfWork.ShoppingCart.GetAll(u =>
                u.ApplicationUserId == cart.ApplicationUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32(SD.SessionCart, cartItemCount);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveItem(int cartId)
        {
            ShoppingCart? cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();

            // Decrement session cart item count
            Int32 cartItemCount = _unitOfWork.ShoppingCart.GetAll(u =>
            u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart, cartItemCount);

            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(double quantity,
            double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else if (quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }

        // GET /Page/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}

