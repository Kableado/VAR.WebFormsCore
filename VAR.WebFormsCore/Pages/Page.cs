using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Controls;

namespace VAR.WebFormsCore.Pages
{
    // TODO: Implement Page
    public class Page : Control, IHttpHandler
    {
        public string Title { get; set; }

        public HttpContext Context { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            StringWriter stringWriter = new();

            Context = context;
            Page = this;

            if (context.Request.Method == "POST")
            {
                _isPostBack = true;
            }

            OnPreInit();
            OnInit();
            OnLoad();

            if (_isPostBack)
            {
                List<Control> controls = ChildsOfType<IReceivePostbackEvent>();
                foreach (Control control in controls)
                {
                    string clientID = control.ClientID;
                    if (context.Request.Form.ContainsKey(clientID))
                    {
                        (control as IReceivePostbackEvent).ReceivePostBack();
                    }
                }
            }

            OnPreRender();
            Render(stringWriter);
            context.Response.Headers.Add("Content-Type", "text/html");
            Context.Response.WriteAsync(stringWriter.ToString());
        }

        private bool _isPostBack = false;

        public bool IsPostBack { get { return _isPostBack; } }
    }
}