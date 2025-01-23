using Microsoft.AspNetCore.Mvc;

namespace CryptoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}