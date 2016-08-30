using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TakagiSugeno.Tools;
using TakagiSugeno.Model.ViewModels;
using TakagiSugeno.Model.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class InputsController : Controller
    {
        private InputsService _inputsService;
        private VariablesService _variablesService;
        public InputsController(InputsService inputsService, VariablesService variablesService)
        {
            _inputsService = inputsService;
            _variablesService = variablesService;
        }
        // GET: /<controller>/
        public IActionResult Details(int? id)
        {
            InputVM vm = new InputVM();
            if (id.HasValue)
            {
                vm = _inputsService.GetInput(id.Value);
            }
            if (Request.IsAjaxRequest())
                return PartialView(vm);
            return View(vm);
        }

        public IActionResult AddVariable()
        {
            VariableVM variable = _variablesService.CreateVariable();
            return PartialView("VariableRow", variable);
        }

        public IActionResult ChangeVariableType(VariableVM variable)
        {
            _variablesService.ChangeVariableType(variable);
            return PartialView("VariableRow", variable);
        }

        public IActionResult RemoveVariable(InputVM viewModel, int variableId)
        {
            _variablesService.RemoveVariable(viewModel, variableId);
            return PartialView("Details", viewModel);
        }

    }
}
