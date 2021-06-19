namespace VAR.WebForms.Common.Controls
{
    // TODO: Implememnt control
    public class TextBox : Control
    {
        public string Text { get; set; }

        public TextBoxMode TextMode { get; set; } = TextBoxMode.Normal;
    }

    public enum TextBoxMode
    {
        Normal,
        Password,
        MultiLine,
    }
}