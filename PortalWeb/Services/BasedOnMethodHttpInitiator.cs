using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace PortalWeb.Services
{
    public sealed class BasedOnMethodHttpInitiator:IBasedOnMethodHttpInitiator
    {
        private readonly IHttpInitiator _initiator;
        public BasedOnMethodHttpInitiator(IHttpInitiator initiator)
        {
            _initiator = initiator;
        }
        public async Task<HttpResponseMessage> Invoke(string url,string method,string mimeType,byte[] content)
        {
            switch (System.Enum.Parse<MethodName>(method))
            {
                case MethodName.GET:
                    return await _initiator.Get(url);
                case MethodName.POST:
                    return await _initiator.Post(url, new StringContent(Encoding.UTF8.GetString(content), Encoding.UTF8, mimeType));
                case MethodName.PUT:
                    return await _initiator.Put(url, new StringContent(Encoding.UTF8.GetString(content), Encoding.UTF8, mimeType));
                case MethodName.PATCH:
                    return await _initiator.Patch(url, new StringContent(Encoding.UTF8.GetString(content), Encoding.UTF8, mimeType));
                default:
                    return await _initiator.Delete(url);
            }
        }
    }
}
