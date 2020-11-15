using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Northwind.Core.HtmlHelpers
{
    public static class NorthwindImageLinkHtmlHelper
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper html, int imageId)
        {
            return new HtmlString($"/images/{imageId}");
        }
    }
}
