using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Logging;
using PortalWeb.Areas.First.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Collections.Generic;
using PortalWeb.Services;
namespace PortalWeb.Areas.First.Controllers
{
    [Area("First")]
    [Authorize(Roles = "USER, ADMIN")]
    public class HomeController : NotifyController
    {
        private readonly IActionRecognizer _recognizer;
        public HomeController(IActionRecognizer recognizer, ILoggerWrapper wrapper) : base(wrapper)
        {
            _recognizer = recognizer;
        }
        [HttpGet]
        [ResponseCache(CacheProfileName ="viewCache")]
        public IActionResult FindEngine()
        {
            return View("htmlengine");
        }
        //[HttpGet]
        //public IActionResult GetItems()
        //{
        //    return View(model: JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(HttpContext.Request.Cookies["Items"]).ToList());
        //}
        //[HttpPost]
        //public IActionResult PostItem([FromForm]KeyValuePair<string,string> item)
        //{
        //    var oldData = JsonSerializer.Deserialize<KeyValuePair<string, string>[]>(HttpContext.Request.Cookies["Items"]).ToList();
        //    oldData.Add(item);
        //    HttpContext.Response.Cookies.Append("Items", JsonSerializer.Serialize(oldData),new CookieOptions() { HttpOnly=false,IsEssential=true,Secure=true,MaxAge = System.TimeSpan.FromSeconds(Program._singleTone.CookieMaxAge)});
        //    return RedirectToAction("GetItems");
        //}
        [HttpGet]
        [ResponseCache(CacheProfileName ="viewCache")]
        public IActionResult Index()
        {
            _logger.LogInformation("In {0} Action was invoked for {1}: {2}", nameof(HomeController), User.Identity.Name, ControllerContext.ActionDescriptor.DisplayName);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToService([FromForm] ServiceWayModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("In {0} model state is invalid for {1}: {2}", nameof(HomeController), User.Identity.Name, JsonSerializer.Serialize(model));
                return View("Index", model);
            }
            _logger.LogInformation("In {0} Action was invoked for {1}: {2}", nameof(HomeController), User.Identity.Name, ControllerContext.ActionDescriptor.DisplayName);
            return RedirectToAction(_recognizer.Recognize(model.Intent), "Service");
        }
    }
}
