using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class ChartsController : Controller
    {
        private ChartsService _service;

        public ChartsController(ChartsService service)
        {
            _service = service;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetChartData(int inputId)
        {
            return Json(_service.GetChartData(inputId));
        }
    }
}
