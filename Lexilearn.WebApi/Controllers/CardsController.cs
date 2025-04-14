using Microsoft.AspNetCore.Mvc;

namespace Lexilearn.WebApi.Controllers;

public class CardsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}