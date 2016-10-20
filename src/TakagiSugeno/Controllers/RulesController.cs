using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Model.Services;
using TakagiSugeno.Tools;

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
            if (Request.IsAjaxRequest())
                return PartialView(vm);
            ViewBag.SystemId = systemId;
            return View(vm);
        }

        public IActionResult NewRule(int systemId, int ruleId)
        {
            return PartialView("RuleRow", _service.CreateNewRule(systemId, ruleId));
        }

        [HttpPost]
        public IActionResult Save([FromBody] List<RuleVM> rules)
        {
            _service.Save(rules);
            return Json("");
        }

        [HttpPost]
        public IActionResult ClearRules(int systemId)
        {
            _service.ClearRules(systemId);
            return Json("");
        }
    }
}
