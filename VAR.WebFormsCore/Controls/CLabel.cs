namespace VAR.WebForms.Common.Controls
{
    // TODO: Implememnt control
    public class CLabel : Control
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

        //public override void RenderBeginTag(HtmlTextWriter writer)
        //{
        //    if (string.IsNullOrEmpty(_tagName) == false)
        //    {
        //        this.AddAttributesToRender(writer);
        //        writer.RenderBeginTag(_tagName);
        //    }
        //    else
        //    {
        //        base.RenderBeginTag(writer);
        //    }
        //}

        #endregion Life cycle
    }
}