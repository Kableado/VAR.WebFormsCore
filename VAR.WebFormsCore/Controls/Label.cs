using System.IO;

namespace VAR.WebFormsCore.Controls
{
    // TODO: Implememnt control
    public class Label : Control
    {
        #region Declarations

        private string _tagName = "span";

        #endregion Declarations

        #region Properties

        public string Tag
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        public string Text { get; set; }

        #endregion Properties

        #region Life cycle

        public override void Render(TextWriter textWriter)
        {
            textWriter.Write("<{0} ", _tagName);
            RenderAttributes(textWriter);
            textWriter.Write(">");

            textWriter.Write(Text);

            base.Render(textWriter);

            textWriter.Write("</{0}>", _tagName);
        }

        #endregion Life cycle
    }
}