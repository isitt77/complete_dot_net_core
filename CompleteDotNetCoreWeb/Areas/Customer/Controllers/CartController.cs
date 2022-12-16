using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CompleteDotNetCoreWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

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
                    includeProperties: "Product")
            };
            foreach (ShoppingCart cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count,
                    cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);

                ShoppingCartViewModel.CartTotalPrice += (cart.Price *
                    cart.Count);
            }
            return View(ShoppingCartViewModel);
        }

        private double GetPriceBasedOnQuantity(double quantity,
            double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else if (quantity < 100)
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

