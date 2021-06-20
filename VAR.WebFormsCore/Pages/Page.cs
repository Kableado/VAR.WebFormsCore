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

            OnPreInit();
            OnInit();
            OnLoad();
            OnPreRender();

            Render(stringWriter);
            context.Response.Headers.Add("Content-Type", "text/html");
            Context.Response.WriteAsync(stringWriter.ToString());
        }

        public bool IsPostBack { get; }
    }
}