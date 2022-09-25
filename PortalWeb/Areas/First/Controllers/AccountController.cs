using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using PortalWeb.Areas.First.Models;
using Microsoft.Extensions.Logging;
using PortalWeb.Services;
using Microsoft.Extensions.Localization;
using PortalWeb.Services.Options;
namespace PortalWeb.Areas.First.Controllers
{
    [Area("First")]
    public sealed class AccountController : NotifyController
    {
        private readonly IFormFileHandler _fileHandler;
        private readonly IIdentityAuthenticator _authenticator;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IPasswordCacher _passwordCacher;
        private readonly IAvatarMemento _avatarMemento;
        public AccountController(IFormFileHandler handler,IIdentityAuthenticator authenticator,IStringLocalizer<AccountController> localizer,IPasswordCacher passwordCacher,IAvatarMemento avatarMemento,ILoggerWrapper wrapper):base(wrapper)
        {
            _fileHandler = handler;
            _authenticator = authenticator;
            _localizer = localizer;
            _passwordCacher = passwordCacher;
            _avatarMemento = avatarMemento;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName ="viewCache")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]AuthorizationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model: model);
            }
            if(await _authenticator.SignInAsync(new AuthenticationOptions() { Email = model.Email,Login =model.Login,Password=model.Password }))
            {
                _logger.LogTrace("User was sign in is {0}", JsonSerializer.Serialize(model));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _passwordCacher.Password = model.Password;
                ControllerContext.ModelState.AddModelError("", _localizer["LoginError"].Value);
                return View(model: model);
            }
        }
        [HttpGet]
        [ResponseCache(CacheProfileName = "viewCache")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]AuthorizationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model: model);
            }
            var avatar = await _fileHandler.ReadAsync(model.Icon);
            if (await _authenticator.SignUpAsync(new AuthenticationOptions() { Email=model.Email,Password =model.Password,Icon =avatar ,Login = model.Login }))
            {
                _logger.LogTrace("User was sign up is Email:{0}, Password:{1}, Avatar Length(bytes):{2}, UserName:{3}!", model.Email,model.Password,avatar.Length,model.Login);
                _avatarMemento.Store(HttpContext,avatar );
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _passwordCacher.Password = model.Password;
                ControllerContext.ModelState.AddModelError("", _localizer["RegisterError"].Value);
                return View(model: model);
            }
        }
    }
}
