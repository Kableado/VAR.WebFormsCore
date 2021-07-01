﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Controls
{
    public class HtmlForm : Control
    {
        private string _method = "post";

        public HtmlForm() { }

        public override void Render(TextWriter textWriter)
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
            sbAction.Append(Page.GetType().Name);

            if (Page.Context.Request.Query.Count > 0)
            {
                sbAction.Append('?');

                foreach (KeyValuePair<string, StringValues> queryParam in Page.Context.Request.Query)
                {
                    sbAction.AppendFormat("&{0}={1}", ServerHelpers.UrlEncode(queryParam.Key), ServerHelpers.UrlEncode(queryParam.Value[0]));
                }
            }

            return sbAction.ToString();
        }
    }
}