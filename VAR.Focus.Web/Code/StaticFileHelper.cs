﻿using System.Collections.Generic;
using System.IO;
using System.Web;

namespace VAR.Focus.Web.Code
{
    public class StaticFileHelper
    {
        private static Dictionary<string, string> _mimeTypeByExtension = new Dictionary<string, string> { {".aac", "audio/aac"},
            {".abw", "application/x-abiword"},
            {".arc", "application/octet-stream"},
            {".avi", "video/x-msvideo"},
            {".azw", "application/vnd.amazon.ebook"},
            {".bin", "application/octet-stream"},
            {".bz", "application/x-bzip"},
            {".bz2", "application/x-bzip2"},
            {".csh", "application/x-csh"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".doc", "application/msword"},
            {".epub", "application/epub+zip"},
            {".gif", "image/gif"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".ico", "image/x-icon"},
            {".ics", "text/calendar"},
            {".jar", "application/java-archive"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".js", "application/javascript"},
            {".json", "application/json"},
            {".mid", "audio/midi"},
            {".midi", "audio/midi"},
            {".mpeg", "video/mpeg"},
            {".mpkg", "application/vnd.apple.installer+xml"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/vnd.oasis.opendocument.spreadsheet"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".oga", "audio/ogg"},
            {".ogv", "video/ogg"},
            {".ogx", "application/ogg"},
            {".png", "image/png"},
            {".pdf", "application/pdf"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".rar", "application/x-rar-compressed"},
            {".rtf", "application/rtf"},
            {".sh", "application/x-sh"},
            {".svg", "image/svg+xml"},
            {".swf", "application/x-shockwave-flash"},
            {".tar", "application/x-tar"},
            {".tiff", "image/tiff"},
            {".tif", "image/tiff"},
            {".ttf", "font/ttf"},
            {".vsd", "application/vnd.visio"},
            {".wav", "audio/x-wav"},
            {".weba", "audio/webm"},
            {".webm", "video/webm"},
            {".webp", "image/webp"},
            {".woff", "font/woff"},
            {".woff2", "font/woff2"},
            {".xhtml", "application/xhtml+xml"},
            {".xls", "application/vnd.ms-excel"},
            {".xml", "application/xml"},
            {".xul", "application/vnd.mozilla.xul+xml"},
            {".zip", "application/zip"},
            {".7z", "application/x-7z-compressed"},
        };

        public static void ResponseStaticFile(HttpContext context, string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            string contentType = null;
            if (_mimeTypeByExtension.ContainsKey(extension))
            {
                contentType = _mimeTypeByExtension[extension];
            }

            context.Response.Clear();

            if (string.IsNullOrEmpty(contentType) == false)
            {
                context.Response.ContentType = contentType;
            }
            context.Response.PrepareCacheableResponse();

            context.Response.Buffer = true;
            context.Response.WriteFile(filePath);
            context.Response.Flush();
            context.Response.Close();
            context.Response.End();
        }

    }
}