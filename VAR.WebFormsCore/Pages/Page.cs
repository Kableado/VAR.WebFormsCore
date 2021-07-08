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
        public string Title { get; set; }

        public HttpContext Context { get; set; }

        private static Encoding _utf8Econding = new UTF8Encoding();

        public async void ProcessRequest(HttpContext context)
        {
            try
            {
                StringWriter stringWriter = new();

                Context = context;
                Page = this;

                if (context.Request.Method == "POST")
                {
                    _isPostBack = true;
                }

                OnPreInit();
                if (context.Response.HasStarted) { return; }

                OnInit();
                if (context.Response.HasStarted) { return; }

                OnLoad();
                if (context.Response.HasStarted) { return; }

                if (_isPostBack)
                {
                    List<Control> controls = ChildsOfType<IReceivePostbackEvent>();
                    foreach (Control control in controls)
                    {
                        string clientID = control.ClientID;
                        if (context.Request.Form.ContainsKey(clientID))
                        {
                            (control as IReceivePostbackEvent).ReceivePostBack();
                            if (context.Response.HasStarted) { return; }
                        }
                    }
                }

                OnPreRender();
                if (context.Response.HasStarted) { return; }

                Render(stringWriter);
                if (context.Response.HasStarted) { return; }

                context.Response.Headers.Add("Content-Type", "text/html");
                byte[] byteObject = _utf8Econding.GetBytes(stringWriter.ToString());
                await context.Response.Body.WriteAsync(byteObject);
            }
            catch (Exception ex)
            {
                // TODO: Implement better error logging
                Console.WriteLine("!!!!!!!!!!");
                Console.Write("Message: {0}\nStacktrace: {1}\n", ex.Message, ex.StackTrace);

                await GlobalErrorHandler.HandleErrorAsync(context, ex);
            }
        }

        private bool _isPostBack = false;

        public bool IsPostBack { get { return _isPostBack; } }
    }
}