using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Model.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class RulesController : Controller
    {
        private RulesService _service;

        public RulesController(RulesService service)
        {
            _service = service;
        }

        public IActionResult SystemRules(int systemId)
        {
            RuleGeneralVM vm = _service.GetSystemRules(systemId);
            ViewBag.SystemId = systemId;
            return View(vm);
        }
    }
}
