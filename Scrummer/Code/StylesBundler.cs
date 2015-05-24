﻿using System.Web;

namespace Scrummer.Code
{
    public class StylesBundler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Bundler bundler = new Bundler(context.Server.MapPath("~/Styles/"));
            context.Response.ContentType = "text/css";
            bundler.WriteResponse(context.Response.OutputStream);
        }
    }
}