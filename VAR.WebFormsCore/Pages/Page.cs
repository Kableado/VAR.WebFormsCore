using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Controls;

namespace VAR.WebFormsCore.Pages
{
    public class Page : Control, IHttpHandler
    {
        protected string Title { get; set; }

        public HttpContext Context { get; private set; }

        private static readonly Encoding Utf8Encoding = new UTF8Encoding();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                StringWriter stringWriter = new();

                Context = context;
                Page = this;

                if (context.Request.Method == "POST") { _isPostBack = true; }

                OnPreInit(EventArgs.Empty);
                if (context.Response.HasStarted) { return; }

                OnInit(EventArgs.Empty);
                if (context.Response.HasStarted) { return; }

                OnLoad(EventArgs.Empty);
                if (context.Response.HasStarted) { return; }

                if (_isPostBack)
                {
                    List<Control> controls = ChildsOfType<IReceivePostbackEvent>();
                    foreach (Control control in controls)
                    {
                        string clientID = control.ClientID;
                        if (context.Request.Form.ContainsKey(clientID))
                        {
                            (control as IReceivePostbackEvent)?.ReceivePostBack();
                            if (context.Response.HasStarted) { return; }
                        }
                    }
                }

                OnPreRender(EventArgs.Empty);
                if (context.Response.HasStarted) { return; }

                Render(stringWriter);
                if (context.Response.HasStarted) { return; }

                context.Response.Headers.SafeSet("Content-Type", "text/html");
                byte[] byteObject = Utf8Encoding.GetBytes(stringWriter.ToString());
                context.Response.Body.WriteAsync(byteObject).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // TODO: Implement better error logging
                Console.WriteLine("!!!!!!!!!!");
                Console.Write("Message: {0}\nStacktrace: {1}\n", ex.Message, ex.StackTrace);

                GlobalErrorHandler.HandleError(context, ex);
            }
        }

        private bool _isPostBack;

        public bool IsPostBack => _isPostBack;
    }
}