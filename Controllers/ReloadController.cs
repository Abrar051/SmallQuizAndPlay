using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.Controllers
{
    public class ReloadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
