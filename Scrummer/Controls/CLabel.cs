using System.Web.UI;
using System.Web.UI.WebControls;

namespace Scrummer.Controls
{
    public class CLabel : Label
    {
        #region Declarations

        private string _tagName = "span";

        #endregion

        #region Properties

        public string Tag
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        #endregion

        #region Life cycle

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(_tagName) == false)
            {
                this.AddAttributesToRender(writer);
                writer.RenderBeginTag(_tagName);
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        #endregion
    }
}
