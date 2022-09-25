using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using System;
namespace PortalWeb
{
    public sealed class TextLogger:ILogger
    {
        public string Path { get; }
        public TextLogger(string path)
        {
            Path = path;
        }
        public bool IsEnabled(LogLevel level) => true;
        public IDisposable BeginScope<TState>(TState state) => null;
        public async void Log<TState>(LogLevel level,EventId id,TState state,Exception ex,Func<TState,Exception,string> formatter)
        {
            using (Stream stream = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.NewLine = Environment.NewLine;
                    await writer.WriteLineAsync($"<{DateTime.Now}> : {level}[{id}] with {JsonSerializer.Serialize(state)} is {(ex is null ? "not faulted" : ex.Message)}");
                }
            }
        }
    }
}
