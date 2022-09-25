using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
namespace PortalWeb
{
    public sealed class BinaryConfigurationProvider : ConfigurationProvider
    {
        public string Path { get; }
        public BinaryConfigurationProvider(string path)
        {
            Path = System.IO.Path.Combine(Environment.CurrentDirectory, path);
        }
        public override void Load()
        {
            Data = new Dictionary<string, string>(5);
            using (Stream stream = File.OpenRead(Path))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    while (reader.PeekChar() != -1)
                    {
                        Data.TryAdd(reader.ReadString(), reader.ReadString());
                    }
                }
            }
        }
    }
    public sealed class BinaryConfigurationSource : IConfigurationSource
    {
        public string Path { get; }
        public BinaryConfigurationSource(string path)
        {
            Path = path;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new BinaryConfigurationProvider(Path);
        }
    }
}
