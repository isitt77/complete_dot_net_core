using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CompleteDotNetCore.Models;
using CompleteDotNetCore.DataAccess.Repository.IRepository;

namespace CompleteDotNetCoreWeb.Controllers;

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

