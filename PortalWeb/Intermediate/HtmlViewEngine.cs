using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
namespace PortalWeb.Intermediate
{
    public sealed class HtmlViewEngine : IViewEngine
    {
        public ViewEngineResult GetView(string exec, string viewParam, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewParam, new string[] { exec, viewParam });
        }
        public ViewEngineResult FindView(ActionContext ctx, string viewParam, bool isMainPage)
        {
            string param;
            if (isMainPage || viewParam == null)
            {
                param = $"Areas\\{ctx.RouteData.Values["area"]}\\Views\\{ctx.RouteData.Values["controller"]}\\Index.html";
            }
            else
            {
                param = $"Areas\\{ctx.RouteData.Values["area"]}\\Views\\{ctx.RouteData.Values["controller"]}\\{viewParam}.html";
            }
            if (File.Exists(param))
            {
                return ViewEngineResult.Found(param, new HtmlView(Path.Combine(Environment.CurrentDirectory, param)));
            }
            else
            {
                return ViewEngineResult.NotFound(param, new string[] { param, viewParam, isMainPage.ToString() });
            }
        }
    }
}
