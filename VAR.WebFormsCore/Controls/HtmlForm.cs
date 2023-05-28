using System.Collections.Generic;
using System.IO;
using System.Text;
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

            if ((Page?.Context?.RequestQuery.Count ?? 0) <= 0) { return sbAction.ToString(); }

            sbAction.Append('?');
            if (Page?.Context?.RequestQuery != null)
            {
                foreach (KeyValuePair<string, string?> queryParam in Page.Context.RequestQuery)
                {
                    string key = ServerHelpers.UrlEncode(queryParam.Key);
                    string value = ServerHelpers.UrlEncode(queryParam.Value ?? string.Empty);
                    sbAction.Append($"&{key}={value}");
                }
            }

            return sbAction.ToString();
        }
    }
}