using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Northwind.Core.TagHelpes
{
    [HtmlTargetElement(Attributes = "northwind-id")]
    public class NorthwindImageTagHelper : TagHelper
    {
        public int NorthwindId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var link = $"/images/{NorthwindId}";
            var content = await output.GetChildContentAsync();

            output.Content.SetHtmlContent($"<img height=\"80\" src=\"{ link }\" alt=\"{content.GetContent()}\" />");
            output.Attributes.SetAttribute("href", link);
            output.Attributes.RemoveAll(nameof(NorthwindId));
        }
    }
}
