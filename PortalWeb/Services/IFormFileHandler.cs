using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace PortalWeb.Services
{
    public interface IFormFileHandler
    {
        Task<byte[]> ReadAsync(IFormFile file);
        byte[] Read(IFormFile file);
    }
}
