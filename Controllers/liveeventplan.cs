using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.Controllers
{
    public class liveeventplan : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult thankyou()
        {
            return View();
        }

        public IActionResult submitinfo(string name, string phone)
        {
            return RedirectToAction("thankyou");
        }

    }
}
