using Microsoft.AspNetCore.Http;
using VAR.WebForms.Common.Code;
using VAR.WebForms.Common.Controls;

namespace VAR.WebForms.Common.Pages
{
    // TODO: Implement Page
    public class Page : Control, IHttpHandler
    {
        public string Title { get; set; }

        public HttpContext Context { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            Context = context;
            throw new System.NotImplementedException();
        }

        public bool IsPostBack { get; }
    }
}