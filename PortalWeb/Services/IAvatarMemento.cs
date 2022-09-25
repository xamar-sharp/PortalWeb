
using Microsoft.AspNetCore.Http;
namespace PortalWeb.Services
{
    public interface IAvatarMemento
    {
        void Store(HttpContext ctx,byte[] icon);
        byte[] TryLoad(HttpContext ctx,out bool result);
    }
}
