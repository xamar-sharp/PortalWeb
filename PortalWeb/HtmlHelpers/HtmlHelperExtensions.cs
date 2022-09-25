using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System;
namespace PortalWeb.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString DisplayList(this IHtmlHelper helper, IList<KeyValuePair<string, string>> source)
        {
            TagBuilder builder = new TagBuilder("table");
            for (int x = 0; x < source.Count; x++)
            {
                TagBuilder tr = new TagBuilder("tr");
                for (int y = 0; y < 2; y++)
                {
                    TagBuilder td = new TagBuilder("td");
                    if (y == 0)
                    {
                        td.InnerHtml.SetContent(source[x].Key);
                    }
                    else
                    {
                        td.InnerHtml.SetContent(source[x].Value);
                    }
                    tr.InnerHtml.AppendHtml(td);
                }
                builder.InnerHtml.AppendHtml(tr);
            }
            StringWriter writer = new StringWriter();
            builder.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}
