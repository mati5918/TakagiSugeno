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
        private OutputCalculator _calc;

        public OutputCalcController(InputsService inputsService, OutputsService outputsService, OutputCalculator calc)
        {
            _calc = calc;
            _inputsService = inputsService;
            _outputsService = outputsService;                
        }
        // GET: /<controller>/
        public IActionResult Index(int systemId)
        {
            OutputCalcVM vm = new OutputCalcVM
            {
                Inputs = _inputsService.GetSystemInputsNames(systemId),
                Outputs = _outputsService.GetSystemOutputsNames(systemId),
                SystemId = systemId
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Calc([FromBody] OutputCalcData data)
        {
            Dictionary<string, double> outputs = _calc.CalcOutputsValues(data);
            return Json(outputs);
        }
    }
}
