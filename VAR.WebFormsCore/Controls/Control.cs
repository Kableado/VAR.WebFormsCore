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

        protected void OnPreInit()
        {
            PreInit?.Invoke(this, null);
            foreach (Control control in Controls)
            {
                control.OnPreInit();
            }
        }


        public event EventHandler Init;

        protected void OnInit()
        {
            Init?.Invoke(this, null);
            foreach (Control control in Controls)
            {
                control.OnInit();
            }
        }


        public event EventHandler Load;

        protected virtual void Process() { }

        protected void OnLoad()
        {
            Process();
            Load?.Invoke(this, null);
            foreach (Control control in Controls)
            {
                control.OnLoad();
            }
        }

        public event EventHandler PreRender;

        protected void OnPreRender()
        {
            PreRender?.Invoke(this, null);
            foreach (Control control in Controls)
            {
                control.OnPreRender();
            }
        }

        private string _id = null;

        public string ID { get { return _id; } set { _id = value; _clientID = null; } }

        private string _clientID = null;

        public string ClientID
        {
            get
            {
                if (_clientID == null)
                {
                    _clientID = GenerateClientID();
                }
                return _clientID;
            }
        }

        private Control _parent = null;

        public Control Parent { get { return _parent; } set { _parent = value; _clientID = null; } }

        private ControlCollection _controls = null;

        private string _cssClass = null;

        public string CssClass { get { return _cssClass; } set { _cssClass = value; } }

        public ControlCollection Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new ControlCollection(this);
                }
                return _controls;
            }
        }

        private Page _page = null;

        public Page Page
        {
            get { return _page; }
            set
            {
                _page = value;
                foreach (Control control in Controls)
                {
                    control.Page = value;
                }
            }
        }

        private bool _visible = true;

        public bool Visible { get { return _visible; } set { _visible = value; } }

        private string GenerateClientID()
        {
            StringBuilder sbClientID = new();

            if (string.IsNullOrEmpty(_id) == false)
            {
                sbClientID.Insert(0, _id);
            }
            else
            {
                string currentID = string.Format("ctl{0:00}", Index);
                sbClientID.Insert(0, currentID);
            }

            Control parent = Parent;
            while (parent != null)
            {
                if (parent is INamingContainer)
                {
                    sbClientID.Insert(0, "_");
                    if (string.IsNullOrEmpty(parent.ID) == false)
                    {
                        sbClientID.Insert(0, parent.ID);
                    }
                    else
                    {
                        string parentID = string.Format("ctl{0:00}", parent.Index);
                        sbClientID.Insert(0, parentID);
                    }
                }

                parent = parent.Parent;
            }

            return sbClientID.ToString();
        }

        public Dictionary<string, string> Style { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        private int _index = 0;

        public int Index { get { return _index; } set { _index = value; } }

        public virtual void Render(TextWriter textWriter)
        {
            foreach (Control control in Controls)
            {
                if (control.Visible == false) { continue; }
                control.Render(textWriter);
            }
        }

        public void RenderAttribute(TextWriter textWriter, string key, string value)
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
            if (string.IsNullOrEmpty(_cssClass) == false)
            {
                RenderAttribute(textWriter, "class", _cssClass);
            }
            foreach (KeyValuePair<string, string> attributePair in Attributes)
            {
                RenderAttribute(textWriter, attributePair.Key, attributePair.Value);
            }
            if (Style.Count > 0)
            {
                StringBuilder sbStyle = new();
                foreach (KeyValuePair<string, string> stylePair in Style)
                {
                    sbStyle.AppendFormat("{0}: {1};", stylePair.Key, stylePair.Value);
                }
                RenderAttribute(textWriter, "style", sbStyle.ToString());
            }
        }

        public List<Control> ChildsOfType<T>(List<Control> controls = null)
        {
            if (controls == null)
            {
                controls = new List<Control>();
            }

            if (this is T) { controls.Add(this); }

            foreach (Control child in _controls)
            {
                child.ChildsOfType<T>(controls);
            }
            return controls;
        }

    }
}