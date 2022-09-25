using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace PortalWeb.Services
{
    public sealed class HttpInitiator:IHttpInitiator
    {
        private static readonly HttpClient _client;
        static HttpInitiator()
        {
            _client = new HttpClient(new SocketsHttpHandler() { MaxConnectionsPerServer = Environment.ProcessorCount, AutomaticDecompression = DecompressionMethods.All,AllowAutoRedirect=true }) { Timeout=TimeSpan.FromSeconds(Program._singleTone.HttpTimeout)};
        }
        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _client.GetAsync(url).ConfigureAwait(false);
        }
        public async Task<HttpResponseMessage> Post(string url,HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }
        public async Task<HttpResponseMessage> Put(string url, HttpContent content)
        {
            return await _client.PutAsync(url, content);
        }
        public async Task<HttpResponseMessage> Patch(string url, HttpContent content)
        {
            return await _client.PatchAsync(url, content);
        }
        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await _client.DeleteAsync(url);
        }
    }
}
