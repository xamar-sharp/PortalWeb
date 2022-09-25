using System.Net.Http;
using PortalWeb.Areas.First.Models;
using System.Threading.Tasks;
namespace PortalWeb.Services
{
    public interface IHttpResponseFormatter
    {
        Task<HttpResponseModel> FormatResponse(HttpResponseMessage msg);
    }
}
