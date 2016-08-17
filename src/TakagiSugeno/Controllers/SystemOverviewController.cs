using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TakagiSugeno.Controllers
{
    public class SystemOverviewController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            TakagiSugeno.Model.ViewModels.SystemVM model = new Model.ViewModels.SystemVM();
            model.Inputs = new List<Model.ViewModels.InputVM>();
            model.Inputs.Add(new Model.ViewModels.InputVM { Name = "A" });
            model.Inputs.Add(new Model.ViewModels.InputVM { Name = "B" });
            return View(model);
        }
    }
}
