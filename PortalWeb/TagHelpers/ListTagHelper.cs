using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
namespace PortalWeb.TagHelpers
{
    [HtmlTargetElement("list", ParentTag = "div", Attributes = "count", TagStructure = TagStructure.NormalOrSelfClosing)]
    public sealed class ListTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public string DisplayValue { get; set; }
        public override async Task ProcessAsync(TagHelperContext ctx,TagHelperOutput output)
        {
            if(DisplayValue is null || ctx.AllAttributes["count"]?.Value is null)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "li";
                output.TagMode = TagMode.StartTagAndEndTag;
                TagHelperContent fromChild = await output.GetChildContentAsync();
                output.PostElement.AppendHtml(fromChild);
                for(int x = 0; x < System.Convert.ToInt32(ctx.AllAttributes["count"].Value); x++)
                {
                    output.Content.AppendHtml("<ol>" + DisplayValue + "</ol>");
                }
            }
            await Task.Yield();
        }
    }
}
