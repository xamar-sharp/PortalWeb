using PortalWeb.Areas.First.Models;
using System.Threading.Tasks;
using System.Text.Json;
namespace PortalWeb.Services
{
    public sealed class HttpResponseFormatter:IHttpResponseFormatter
    {
        public async Task<HttpResponseModel> FormatResponse(System.Net.Http.HttpResponseMessage msg)
        {
            return new HttpResponseModel(msg.IsSuccessStatusCode, await msg.Content.ReadAsStringAsync(), msg.Content.Headers.ContentType.MediaType, 2, JsonSerializer.Serialize(msg.Headers));
        }
    }
}
