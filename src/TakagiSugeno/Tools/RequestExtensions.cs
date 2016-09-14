using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Tools
{
    public static class RequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static IList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> EnumTest()
        {
            IList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> res = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            var items =  new [] { Model.VariableType.Trapeze, Model.VariableType.Triangle };
            foreach (var item in items)
            {
                res.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = item.ToString(), Value = item.ToString() });
            }
            return res;
        }
    }
}
