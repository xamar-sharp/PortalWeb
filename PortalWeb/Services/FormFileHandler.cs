using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace PortalWeb.Services
{
    public sealed class FormFileHandler:IFormFileHandler
    {
        public async Task<byte[]> ReadAsync(IFormFile file)
        {
            byte[] result;
            using(Stream stream = file.OpenReadStream())
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result);
            }
            return result;
        }
        public byte[] Read(IFormFile file)
        {
            byte[] result;
            using (Stream stream = file.OpenReadStream())
            {
                result = new byte[stream.Length];
                stream.Read(result);
            }
            return result;
        }
    }
}
