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
            return View(ShoppingCartViewModel);
        }
    }
}

