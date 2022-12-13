using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CompleteDotNetCoreWeb.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> objProductList =
            _unitOfWork.Product.GetAll(includeProperties:
            "Category,CoverType");
        return View(objProductList);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCart shoppingCartObj = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitOfWork.Product.GetFirstOrDefault(
            u => u.Id == productId, includeProperties: "Category,CoverType")
        };

        return View(shoppingCartObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
        Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.ApplicationUserId = claim.Value;

        Console.WriteLine("ShoppingCart Id: " + shoppingCart.Id);
        Console.WriteLine("ProductId: " + shoppingCart.ProductId);
        //Console.WriteLine("user: " + shoppingCart.ApplicationUserId);

        //_unitOfWork.ShoppingCart.Add(shoppingCart);
        //_unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

