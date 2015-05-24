﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scrummer.Code.JSON;

namespace Scrummer.Code.Pages
{
    public class FrmEcho : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var jsonWritter = new JSONWriter(true);
            context.Response.Write("<pre><code>");
            context.Response.Write(jsonWritter.Write(context.Request));
            context.Response.Write("</code></pre>");
        }
    }
}