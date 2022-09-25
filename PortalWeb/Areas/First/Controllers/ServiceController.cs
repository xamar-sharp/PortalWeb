using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using PortalWeb.Services;
using System.Threading.Tasks;
using PortalWeb.Areas.First.Models;
using System.Text.Json;
namespace PortalWeb.Areas.First.Controllers
{
    [Area("First")]
    [Authorize(Roles ="USER, ADMIN")]
    public sealed class ServiceController : NotifyController
    {
        private readonly IBasedOnMethodHttpInitiator _init;
        private readonly IHttpResponseFormatter _formatter;
        private readonly IFormFileHandler _fileHandler;
        private readonly CommentManager _manager;
        public ServiceController(IBasedOnMethodHttpInitiator init, IHttpResponseFormatter format,IFormFileHandler handler,CommentManager manager,ILoggerWrapper wrapper):base(wrapper)
        {
            _init = init;
            _formatter = format;
            _fileHandler = handler;
            _manager = manager;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(string intent,string title,string description,string rateLevel)
        {
            if(await _manager.AddAsync(User.Identity.Name, new Comment() { Title = title, Description = description, RateLevel = System.Enum.Parse<RateLevel>(rateLevel).ToString(), Intent = intent }))
            {
                return Ok();
            }
            return BadRequest();
        }
        [AcceptVerbs("GET","DELETE")]
        [ResponseCache(CacheProfileName = "viewCache")]
        public IActionResult ServiceNotFound()
        {
            _logger.LogInformation("In {0} Action was invoked for {1}: {2}",nameof(ServiceController), User.Identity.Name,ControllerContext.ActionDescriptor.DisplayName);
            return View();
        }
        [HttpGet]
        [ResponseCache(CacheProfileName ="viewCache")]
        public IActionResult HttpInvoke()
        {
            _logger.LogInformation("In {0} Action was invoked for {1}: {2}",nameof(ServiceController), User.Identity.Name, ControllerContext.ActionDescriptor.DisplayName);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HttpInvoke([FromForm] HttpRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("In {0} model state is invalid for {1}: {2}", nameof(ServiceController), User.Identity.Name, JsonSerializer.Serialize(model));
                return View(model: model);
            }
            byte[] data = null;
            if (model.Content != null)
            {
                data = await _fileHandler.ReadAsync(model.Content);
            }
            _logger.LogInformation("In {0} Action was invoked for {1}: {2}", nameof(ServiceController), User.Identity.Name, ControllerContext.ActionDescriptor.DisplayName);
            return View("FormatHttp", await _formatter.FormatResponse(await _init.Invoke(model.Url, model.Method, model.MimeType, data)));
        }
    }
}
