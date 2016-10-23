using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.Services;
using TakagiSugeno.Model.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class SystemsController : Controller
    {
        private SystemsService _service;

        public SystemsController(SystemsService service)
        {
            _service = service;
        }
        // GET: /<controller>/
        public IActionResult SystemsList(int? openedSystem)
        {
            ViewBag.SystemId = openedSystem;
            return View(_service.GetSystems());
        }

        public IActionResult New()
        {
            int newId = _service.CreateSystem();
            return RedirectToAction("Index", "SystemOverview", new { systemId = newId });
        }

        public IActionResult Publish([FromBody] PublishVM publishData)
        {
            _service.Publish(publishData);
            return Json("");
        }
    }
}
