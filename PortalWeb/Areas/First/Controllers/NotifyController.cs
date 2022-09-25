using Microsoft.AspNetCore.Mvc;
using PortalWeb.Services;
using Microsoft.Extensions.Logging;
namespace PortalWeb.Areas.First.Controllers
{
    public class NotifyController:Controller
    {
        protected readonly ILogger _logger;
        public NotifyController(ILoggerWrapper wrapper)
        {
            _logger = wrapper.Unwrap();
        }
    }
}
