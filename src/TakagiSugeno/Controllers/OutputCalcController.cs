using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Model.Services;
using TakagiSugeno.Model;
using TakagiSugeno.Model.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class OutputCalcController : Controller
    {
        private InputsService _inputsService;
        private OutputsService _outputsService;
        private SystemsService _systemsService;
        private OutputCalculator _calc;

        public OutputCalcController(InputsService inputsService, OutputsService outputsService, SystemsService systemsService, OutputCalculator calc)
        {
            _calc = calc;
            _inputsService = inputsService;
            _outputsService = outputsService;
            _systemsService = systemsService;              
        }
        // GET: /<controller>/
        public IActionResult Index(int systemId)
        {
            OutputCalcVM vm = new OutputCalcVM
            {
                Inputs = _inputsService.GetSystemInputsNames(systemId),
                Outputs = _outputsService.GetSystemOutputsNames(systemId),
                SystemId = systemId,
                AndMethod = _systemsService.GetSystemAndMethod(systemId),
                OrMethod = _systemsService.GetSystemOrMethod(systemId)
            };
            ViewBag.SystemId = systemId;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Calc([FromBody] OutputCalcData data)
        {
            _systemsService.ModifySystemMethods(data.SystemId, data.AndMethod, data.OrMethod);
            Dictionary<string, double> outputs = _calc.CalcOutputsValues(data);
            return Json(outputs);
        }
    }
}
