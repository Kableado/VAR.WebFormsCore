using System.IO;

namespace VAR.WebFormsCore.Controls
{
    public class HtmlHead : Control
    {
        public string Title { get; set; }

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<head ");
            RenderAttributes(textWriter);
            textWriter.Write(">");

            if (string.IsNullOrEmpty(Title) == false)
            {
                textWriter.Write("<title>{0}</title>", Title);
            }

            base.Render(textWriter);

            textWriter.Write("</head>");
        }
    }
}