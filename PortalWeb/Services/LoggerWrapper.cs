using Microsoft.Extensions.Logging;
namespace PortalWeb.Services
{
    public sealed class LoggerWrapper : ILoggerWrapper
    {
        private static readonly ILogger _singleTone = new TextLogger(Program._singleTone.LoggingPath);
        public ILogger Unwrap()
        {
            return _singleTone;
        }
    }
}
