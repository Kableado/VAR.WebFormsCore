using System;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Scrummer.Code.Controls;

namespace Scrummer.Code.Pages
{
    public class PageCommon : Page
    {
        #region Declarations

        private HtmlHead _head;
        private HtmlGenericControl _body;
        private HtmlForm _form;
        private Panel _pnlContainer = new Panel();

        #endregion

        #region Properties

        public new ControlCollection Controls
        {
            get { return _pnlContainer.Controls; }
        }

        #endregion

        #region Life cycle

        public PageCommon()
        {
            Init += PageCommon_Init;
            PreRender += PageCommon_PreRender;
        }

        void PageCommon_Init(object sender, EventArgs e)
        {
            CreateControls();
        }

        void PageCommon_PreRender(object sender, EventArgs e)
        {
            _head.Title = string.IsNullOrEmpty(Title) ? Globals.Title : String.Format("{0}{1}{2}", Globals.Title, Globals.TitleSeparator, Title);
        }

        #endregion

        #region Private methods

        private void CreateControls()
        {
            Context.Response.Charset = Encoding.UTF8.WebName;

            var doctype = new LiteralControl("<!DOCTYPE html>\n");
            base.Controls.Add(doctype);

            var html = new HtmlGenericControl("html");
            base.Controls.Add(html);

            _head = new HtmlHead();
            html.Controls.Add(_head);

            _head.Controls.Add(new HtmlMeta { HttpEquiv = "content-type", Content = "text/html; charset=utf-8" });
            _head.Controls.Add(new HtmlMeta { Name = "author", Content = Globals.Author });
            _head.Controls.Add(new HtmlMeta { Name = "Copyright", Content = Globals.Copyright });
            Assembly asm = Assembly.GetExecutingAssembly();
            string version = asm.GetName().Version.ToString();
            _head.Controls.Add(new LiteralControl(String.Format("<script type=\"text/javascript\" src=\"ScriptsBundler?t={0}\"></script>\n", version)));
            _head.Controls.Add(new LiteralControl(String.Format("<link href=\"StylesBundler?t={0}\" type=\"text/css\" rel=\"stylesheet\"/>\n", version)));


            _body = new HtmlGenericControl("body");
            html.Controls.Add(_body);
            _form = new HtmlForm { ID = "formMain" };
            _body.Controls.Add(_form);

            var pnlHeader = new Panel { CssClass = "divHeader" };
            _form.Controls.Add(pnlHeader);

            var lblTitle = new CLabel { Text = Globals.Title, Tag = "h1" };
            pnlHeader.Controls.Add(lblTitle);

            _pnlContainer.CssClass = "divContent";
            _form.Controls.Add(_pnlContainer);
        }

        #endregion
    }
}
