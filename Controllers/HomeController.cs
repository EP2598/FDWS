using Microsoft.AspNetCore.Mvc;
using FDWS.Services;
using FDWS.Models;
using System.Threading.Tasks;

namespace FDWS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBusinessLogic _businessLogic;

        public HomeController(IBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _businessLogic.GetHomeDataAsync();
            var model = new HomeViewModel
            {
                Title = "Welcome to FDWS",
                Message = "Your application is running successfully!",
                Timestamp = System.DateTime.Now
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData(int id)
        {
            var result = await _businessLogic.ProcessDataAsync(id);
            return Json(result);
        }
    }
}