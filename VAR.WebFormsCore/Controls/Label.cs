using System.IO;
using VAR.WebFormsCore.Code;

namespace VAR.WebFormsCore.Controls
{
    public class Label : Control
    {
        #region Properties

        private string _tagName = "span";

        public string Tag
        {
            get => _tagName;
            set => _tagName = value;
        }

        private string _text = string.Empty;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        #endregion Properties

        #region Life cycle

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<{0} ", _tagName);
            RenderAttributes(textWriter);
            textWriter.Write(">");

            textWriter.Write(ServerHelpers.HtmlEncode(_text));

            base.Render(textWriter);

            textWriter.Write("</{0}>", _tagName);
        }

        #endregion Life cycle
    }
}