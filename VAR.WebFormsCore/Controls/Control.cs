using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Pages;

namespace VAR.WebFormsCore.Controls
{
    public class Control
    {
        public event EventHandler PreInit;

        protected void OnPreInit(EventArgs e)
        {
            PreInit?.Invoke(this, e);
            foreach (Control control in Controls) { control.OnPreInit(e); }
        }


        public event EventHandler Init;

        protected void OnInit(EventArgs e)
        {
            Init?.Invoke(this, e);
            foreach (Control control in Controls) { control.OnInit(e); }
        }


        public event EventHandler Load;

        protected virtual void Process() { }

        protected void OnLoad(EventArgs e)
        {
            Process();
            Load?.Invoke(this, e);
            foreach (Control control in Controls) { control.OnLoad(e); }
        }

        public event EventHandler PreRender;

        protected void OnPreRender(EventArgs e)
        {
            PreRender?.Invoke(this, e);
            foreach (Control control in Controls) { control.OnPreRender(e); }
        }

        private string _id;

        public string ID
        {
            get => _id;
            set
            {
                _id = value;
                _clientID = null;
            }
        }

        private string _clientID;

        public string ClientID
        {
            get { return _clientID ??= GenerateClientID(); }
        }

        private Control _parent;

        public Control Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                _clientID = null;
            }
        }

        private ControlCollection _controls;

        public string CssClass { get; set; }

        public ControlCollection Controls
        {
            get { return _controls ??= new ControlCollection(this); }
        }

        private Page _page;

        public Page Page
        {
            get => _page;
            set
            {
                _page = value;
                foreach (Control control in Controls) { control.Page = value; }
            }
        }

        public bool Visible { get; set; } = true;

        private string GenerateClientID()
        {
            StringBuilder sbClientID = new();

            if (string.IsNullOrEmpty(_id) == false) { sbClientID.Insert(0, _id); }
            else
            {
                string currentID = $"ctl{Index:00}";
                sbClientID.Insert(0, currentID);
            }

            Control parent = Parent;
            while (parent != null)
            {
                if (parent is INamingContainer)
                {
                    sbClientID.Insert(0, "_");
                    if (string.IsNullOrEmpty(parent.ID) == false) { sbClientID.Insert(0, parent.ID); }
                    else
                    {
                        string parentID = $"ctl{parent.Index:00}";
                        sbClientID.Insert(0, parentID);
                    }
                }

                parent = parent.Parent;
            }

            return sbClientID.ToString();
        }

        public Dictionary<string, string> Style { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public int Index { get; set; }

        protected virtual void Render(TextWriter textWriter)
        {
            foreach (Control control in Controls)
            {
                if (control.Visible == false) { continue; }

                control.Render(textWriter);
            }
        }

        protected static void RenderAttribute(TextWriter textWriter, string key, string value)
        {
            textWriter.Write(" {0}=\"{1}\"", key, ServerHelpers.HtmlEncode(value));
        }

        protected void RenderAttributes(TextWriter textWriter, bool forceId = false)
        {
            if (string.IsNullOrEmpty(_id) == false || forceId)
            {
                RenderAttribute(textWriter, "id", ClientID);
                RenderAttribute(textWriter, "name", ClientID);
            }

            if (string.IsNullOrEmpty(CssClass) == false) { RenderAttribute(textWriter, "class", CssClass); }

            foreach (KeyValuePair<string, string> attributePair in Attributes)
            {
                RenderAttribute(textWriter, attributePair.Key, attributePair.Value);
            }

            if (Style.Count > 0)
            {
                StringBuilder sbStyle = new();
                foreach (KeyValuePair<string, string> stylePair in Style)
                {
                    sbStyle.Append($"{stylePair.Key}: {stylePair.Value};");
                }

                RenderAttribute(textWriter, "style", sbStyle.ToString());
            }
        }

        protected List<Control> ChildsOfType<T>(List<Control> controls = null)
        {
            controls ??= new List<Control>();

            if (this is T) { controls.Add(this); }

            foreach (Control child in _controls) { child.ChildsOfType<T>(controls); }

            return controls;
        }
    }
}