using Microsoft.AspNetCore.ResponseCompression;
using System.IO;
using System.IO.Compression;
namespace PortalWeb
{
    public sealed class DeflateCompressionProvider : ICompressionProvider
    {
        public string EncodingName => "deflate";
        public bool SupportsFlush => true;
        public Stream CreateStream(Stream stream)
        {
            return new DeflateStream(stream,CompressionLevel.Fastest);
        }
    }
}
