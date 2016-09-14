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
            OutputVM vm = new OutputVM();
            if (id.HasValue)
            {
                vm = _outputsService.GetOutput(id.Value);
            }
            if (Request.IsAjaxRequest())
                return PartialView("OutputDetails", vm);
            return View("OutputDetails", vm);
        }

    }
}
