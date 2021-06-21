using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implment this control
    public class HtmlForm : Control
    {
        private string _method = "post";

        public HtmlForm() { }

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<form ");
            RenderAttributes(textWriter);
            RenderAttribute(textWriter, "method", _method);
            RenderAttribute(textWriter, "action", Page.GetType().Name);
            textWriter.Write(">");

            base.Render(textWriter);

            textWriter.Write("</form>");
        }
    }
}