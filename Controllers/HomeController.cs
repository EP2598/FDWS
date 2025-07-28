using Microsoft.AspNetCore.Mvc;
using FDWS.Services;
using FDWS.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

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
                Message = "Let's calculate how many average villager dies",
                Timestamp = System.DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetResult([FromBody] List<int[]> listInputs)
        {
            var result = await _businessLogic.ValidateInputAsync(listInputs);

            if (result.Status == HttpStatusCode.BadRequest.ToString())
            {
                return Json(new { success = false, message = result.Result });
            }

            return Json(new { success = true, message = result.Result });
        }
    }
}