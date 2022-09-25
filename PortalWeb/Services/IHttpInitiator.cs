using System.Net.Http;
using System.Threading.Tasks;
namespace PortalWeb.Services
{
    public interface IHttpInitiator
    {
        Task<HttpResponseMessage> Get(string url);
        Task<HttpResponseMessage> Post(string url, HttpContent content);
        Task<HttpResponseMessage> Put(string url, HttpContent content);
        Task<HttpResponseMessage> Patch(string url, HttpContent content);
        Task<HttpResponseMessage> Delete(string url);

    }
}
