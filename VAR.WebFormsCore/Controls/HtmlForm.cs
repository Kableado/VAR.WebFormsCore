using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Controls
{
    public class HtmlForm : Control
    {
        private readonly string _method = "post";

        protected override void Render(TextWriter textWriter)
        {
            textWriter.Write("<form ");
            RenderAttributes(textWriter);
            RenderAttribute(textWriter, "method", _method);
            RenderAttribute(textWriter, "action", GetAction());
            textWriter.Write(">");

            base.Render(textWriter);

            textWriter.Write("</form>");
        }

        private string GetAction()
        {
            StringBuilder sbAction = new();
            sbAction.Append(Page?.GetType().Name);

            if ((Page?.Context?.Request.Query.Count ?? 0) <= 0) { return sbAction.ToString(); }

            sbAction.Append('?');
            if (Page?.Context?.Request.Query != null)
            {
                foreach (KeyValuePair<string, StringValues> queryParam in Page.Context.Request.Query)
                {
                    string key = ServerHelpers.UrlEncode(queryParam.Key);
                    string value = ServerHelpers.UrlEncode(queryParam.Value[0] ?? string.Empty);
                    sbAction.Append($"&{key}={value}");
                }
            }

            return sbAction.ToString();
        }
    }
}