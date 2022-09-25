namespace PortalWeb.Areas.First.Models
{
    public sealed class HttpResponseModel
    {
        public bool Success { get; }
        public string Content { get; }
        public string ContentType { get; }
        public long ContentLength { get; }
        public string Headers { get; }
        public HttpResponseModel(bool success,string content,string ctype,long clength,string headers)
        {
            Success = success;
            Content = content;
            ContentType = ctype;
            ContentLength = clength;
            Headers = headers;
        }
    }
}
