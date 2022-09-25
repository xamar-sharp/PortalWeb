using System.Net.Http;
using System.Threading.Tasks;
namespace PortalWeb.Services
{
    public interface IBasedOnMethodHttpInitiator
    {
        Task<HttpResponseMessage> Invoke(string url,string method, string mimeType, byte[] content);
    }
}
