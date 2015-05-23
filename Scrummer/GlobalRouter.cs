using System.Web;

namespace Scrummer
{
    public class GlobalRouter : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("Hello world!");
        }
    }
}