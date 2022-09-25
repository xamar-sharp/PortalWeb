using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace PortalWeb.Services
{
    public sealed class AvatarMemento:IAvatarMemento
    {
        public void Store(HttpContext ctx,byte[] content)
        {
            lock (this)
            {
                if (ctx.Session.IsAvailable)
                {
                    if (ctx.Session.Keys.Contains("Avatar"))
                    {
                        ctx.Session.Remove("Avatar");
                    }
                    ctx.Session.Set("Avatar", content);
                }
            }
        }
        public byte[] TryLoad(HttpContext ctx,out bool result)
        {
            lock (this)
            {
                if (ctx.Session.IsAvailable && ctx.Session.Keys.Contains("Avatar"))
                {
                    result = true;
                    return ctx.Session.Get("Avatar");
                }
                else
                {
                    result = false;
                    return System.Array.Empty<byte>();
                }
            }
        }
    }
}
