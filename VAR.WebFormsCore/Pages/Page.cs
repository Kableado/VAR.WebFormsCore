using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VAR.WebFormsCore.Code;
using VAR.WebFormsCore.Controls;

namespace VAR.WebFormsCore.Pages;

public class Page : Control, IHttpHandler
{
    protected string Title { get; set; } = string.Empty;

    public IWebContext? Context { get; private set; }

    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    public void ProcessRequest(IWebContext context)
    {
        try
        {
            StringWriter stringWriter = new();

            Context = context;
            Page = this;

            if (context.RequestMethod == "POST") { _isPostBack = true; }

            OnPreInit(EventArgs.Empty);
            if (context.ResponseHasStarted) { return; }

            OnInit(EventArgs.Empty);
            if (context.ResponseHasStarted) { return; }

            OnLoad(EventArgs.Empty);
            if (context.ResponseHasStarted) { return; }

            if (_isPostBack)
            {
                List<Control> controls = ChildsOfType<IReceivePostbackEvent>();
                foreach (Control control in controls)
                {
                    string clientID = control.ClientID;
                    if (context.RequestForm.ContainsKey(clientID))
                    {
                        (control as IReceivePostbackEvent)?.ReceivePostBack();
                        if (context.ResponseHasStarted) { return; }
                    }
                }
            }

            OnPreRender(EventArgs.Empty);
            if (context.ResponseHasStarted) { return; }

            Render(stringWriter);
            if (context.ResponseHasStarted) { return; }

            context.ResponseContentType = "text/html";
            byte[] byteObject = Utf8Encoding.GetBytes(stringWriter.ToString());
            context.ResponseWriteBin(byteObject);
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