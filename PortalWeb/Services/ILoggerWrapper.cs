using Microsoft.Extensions.Logging;
namespace PortalWeb.Services
{
    public interface ILoggerWrapper
    {
        ILogger Unwrap();
    }
}
