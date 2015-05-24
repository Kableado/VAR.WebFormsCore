using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrummer.Code
{
    public class ScriptsBundler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(context.Server.MapPath("~/Scripts/"));
            context.Response.ContentType = "text/javascript";
            bundler.WriteResponse(context.Response.OutputStream);
        }
    }
}