using System.IO;

namespace VAR.WebFormsCore.Controls
{
    public class HtmlMeta : Control
    {
        public string Name { get; init; }
        public string Content { get; init; }
        public string HttpEquiv { get; internal init; }

        protected override void Render(TextWriter textWriter)
        {
            textWriter.Write("<meta ");
            RenderAttributes(textWriter);
            if (string.IsNullOrEmpty(Name) == false) { textWriter.Write(" name=\"{0}\"", Name); }

            if (string.IsNullOrEmpty(Content) == false) { textWriter.Write(" content=\"{0}\"", Content); }

            if (string.IsNullOrEmpty(HttpEquiv) == false) { textWriter.Write(" http-equiv=\"{0}\"", HttpEquiv); }

            textWriter.Write(" />");
        }
    }
}