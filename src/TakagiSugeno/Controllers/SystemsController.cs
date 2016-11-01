using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.Services;
using TakagiSugeno.Model.ViewModels;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class SystemsController : Controller
    {
        private SystemsService _service;
        private SystemStateHelper _stateHelper;

        public SystemsController(SystemsService service, SystemStateHelper stateHelper)
        {
            _service = service;
            _stateHelper = stateHelper;
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

        public IActionResult Clone(int systemId)
        {
            int newId = _service.CloneSystem(systemId);
            return RedirectToAction("Index", "SystemOverview", new { systemId = newId });
        }
        [HttpPost]
        public IActionResult IsSystemPublished(int systemId)
        {
            return Json(_stateHelper.IsSystemPublished(systemId));
        }

        public IActionResult SaveToFile(int systemId)
        {
            string systemJson = _service.SystemToJson(systemId);
            return File(Encoding.UTF8.GetBytes(systemJson), "application/json", "TakagiSugenoSystem.json");
        }

        [HttpPost]
        public IActionResult ReadFromFile()
        {
            var file = Request.Form.Files.GetFile("system");
            int? res = _service.ReadFromFile(file);
            return Json(res);
        }
    }
}
