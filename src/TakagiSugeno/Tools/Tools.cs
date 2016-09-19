using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakagiSugeno.Model;

namespace TakagiSugeno.Tools
{
    public static class Tools
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>(this IHtmlHelper helper, IOType type)
        {
            Type t = typeof(T);           
            var list = helper.GetEnumSelectList(t);
            List<string> outputValues = new List<string> { "100", "101" };
            if(type == IOType.Input)
            {
                list = list.Where(i => !outputValues.Contains(i.Value));
            }
            else if(type == IOType.Output)
            {
                list = list.Where(i => outputValues.Contains(i.Value));
            }
            return list;
        }

        public static string DisplayName(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return displayName.Name;
            }
            return value.ToString();
        }

        public static bool IsNameValid(string name, string type, List<string> validationErros)
        {
            if (string.IsNullOrEmpty(name))
            {
                validationErros.Add($"Nazwa {type} nie może być pusta");
                return false;
            }
            if (name.Length > 30)
            {
                validationErros.Add($"Nazwa {type} nie może być dłuższa niż 30 znaków");
                return false;
            }
            if (!Regex.IsMatch(name, @"^[-a-zA-Z0-9]*$"))
            {
                validationErros.Add($"Nazwa {type} zawiera niedozwolone znaki");
                return false;
            }
            return true;
        }
    }
}
