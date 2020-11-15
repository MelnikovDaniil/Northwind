using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Northwind.ViewComponents
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(HttpContext context)
        {
            var breadcrumbs = await ConfigureBreadcrumb(context);
            return View(breadcrumbs);
        }

        private async Task<List<Breadcrumb>> ConfigureBreadcrumb(HttpContext context)
        {
            var breadcrumbList = new List<Breadcrumb>();
            var homeControllerName = "Home";

            breadcrumbList.Add(new Breadcrumb
            {
                Text = "Home",
                Action = "Index",
                Controller = homeControllerName,
                Active = true
            });

            if (context.Request.Path.HasValue)
            {
                var pathSplit = context.Request.Path.Value.Split("/");

                for (var i = 0; i < pathSplit.Length; i++)
                {
                    if (string.IsNullOrEmpty(pathSplit[i]) || string.Compare(pathSplit[i], homeControllerName, true) == 0)
                    {
                        continue;
                    }

                    var controller = this.GetControllerType(pathSplit[i] + "Controller");

                    if (controller != null)
                    {
                        var indexMethod = controller.GetMethod("Index");

                        if (indexMethod != null)
                        {
                            breadcrumbList.Add(new Breadcrumb
                            {
                                Text = this.CamelCaseSpacing(pathSplit[i]),
                                Action = "Index",
                                Controller = pathSplit[i],
                                Active = true
                            });

                            if (i + 1 < pathSplit.Length && string.Compare(pathSplit[i + 1], "Index", true) == 0)
                            {
                                breadcrumbList.LastOrDefault().Active = false;

                                return breadcrumbList;
                            }
                        }
                    }

                    if (i - 1 > 0)
                    {
                        var controllerName = pathSplit[i - 1] + "Controller";
                        var prevController = this.GetControllerType(controllerName);

                        if (prevController != null)
                        {
                            var method = prevController.GetMethods().First(x => x.Name == pathSplit[i]);

                            if (method != null)
                            {
                                breadcrumbList.Add(new Breadcrumb
                                {
                                    Text = this.CamelCaseSpacing(pathSplit[i]),
                                    Action = pathSplit[i],
                                    Controller = pathSplit[i - 1]
                                });
                            }
                        }
                    }
                }
            }

            breadcrumbList.LastOrDefault().Active = false;

            return await Task.FromResult(breadcrumbList);
        }

        private Type GetControllerType(string name)
        {
            Type controller = null;

            try
            {
                controller = Assembly.GetCallingAssembly().GetType("Northwind.Controllers." + name);
            }
            catch
            { }

            return controller;
        }

        private string CamelCaseSpacing(string s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(s, " ");
        }
    }
}
