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

            ShoppingCartViewModel.OrderHeader.PaymentStatus =
                SD.PaymentStatusPending;
            ShoppingCartViewModel.OrderHeader.OrderStatus =
                SD.StatusPending;
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

            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartViewModel.CartList);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Home");
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
    }
}

