using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.Services;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Tools;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class OutputsController : Controller
    {

        private OutputsService _outputsService;
        private VariablesService _variablesService;
        public OutputsController(OutputsService outputsService, VariablesService variablesService)
        {
            _outputsService = outputsService;
            _variablesService = variablesService;
        }

        public IActionResult OutputsList(int systemId)
        {
            return PartialView("List", _outputsService.GetSystemOutputs(systemId));
        }

        public IActionResult Details(int? id)
        {
            OutputVM vm = new OutputVM() { Variables = new List<VariableVM>(), OutputId = -1 };
            if (id.HasValue && id.Value != -1)
            {
                vm = _outputsService.GetOutput(id.Value);
            }
            if (Request.IsAjaxRequest())
                return PartialView("OutputDetails", vm);
            ViewBag.SystemId = vm.SystemId;
            return View("OutputDetails", vm);
        }

        public IActionResult SystemOutputs(int systemId)
        {
            int outputId = _outputsService.FirstSystemOutput(systemId);
            if (outputId != -1)
                return RedirectToAction("Details", new { id = outputId });
            else
                return RedirectToAction("Add", new { systemId = systemId });
        }

        public IActionResult AddVariable(int fakeId)
        {
            VariableVM variable = _variablesService.CreateVariable(fakeId, Model.IOType.Output);
            return PartialView("~/Views/Inputs/VariableRow.cshtml", variable);
        }

        [HttpPost]
        public IActionResult ChangeVariableType(VariableVM variable, int? systemId)
        {
            if (systemId.HasValue)
            {
                _variablesService.ChangeVariableType(variable, systemId.Value);
            }
            return PartialView("~/Views/Inputs/VariableRow.cshtml", variable);
        }

        [HttpPost]
        public IActionResult Save([FromBody] OutputVM viewModel)
        {
            SaveResult res = _outputsService.Save(viewModel);
            return Json(res);
        }

        public IActionResult Remove(int id)
        {
            _outputsService.Remove(id);
            return Json(string.Empty);
        }

        public IActionResult Add(int systemId)
        {
            ViewBag.IsNewOutput = true;
            OutputVM vm = _outputsService.AddOutput(systemId);
            if (Request.IsAjaxRequest())
                return PartialView("OutputDetails", vm);
            ViewBag.SystemId = vm.SystemId;
            return View("OutputDetails", vm);
        }

    }
}
