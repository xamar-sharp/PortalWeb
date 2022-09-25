using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.IO;
namespace PortalWeb.Intermediate
{
    public sealed class HtmlView : IView
    {
        public string Path { get; }
        public HtmlView(string path)
        {
            Path = path;
        }
        public async Task RenderAsync(ViewContext ctx)
        {
            string html = await File.ReadAllTextAsync(Path);
            await ctx.Writer.WriteAsync(html);
        }
    }
}
