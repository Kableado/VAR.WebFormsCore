﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace VAR.Focus.Web.Code
{
    public class Bundler
    {
        #region Declarations

        private string _path = null;
        private List<string> _files = null;

        #endregion Declarations

        #region Properties

        private List<string> Files
        {
            get
            {
                if (_files != null) { return _files; }

                DirectoryInfo dir = new DirectoryInfo(_path);
                FileInfo[] files = dir.GetFiles();
                _files = files.OrderBy(file => file.FullName).Select(file2 => file2.FullName).ToList();
                return _files;
            }
        }

        #endregion Properties

        #region Creator

        public Bundler(string path)
        {
            _path = path;
        }

        #endregion Creator

        #region Public methods

        public void WriteResponse(HttpResponse response, string contentType)
        {
            response.ContentType = contentType;
            foreach (string fileName in Files)
            {
                string fileContent = File.ReadAllText(fileName);
                byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
                if (byteArray.Length > 0)
                {
                    response.OutputStream.Write(byteArray, 0, byteArray.Length);

                    byteArray = Encoding.UTF8.GetBytes("\n\n");
                    response.OutputStream.Write(byteArray, 0, byteArray.Length);
                }
            }
        }

        public void PrepareCacheableResponse(HttpResponse response)
        {
            const int secondsInDay = 86400;
            response.ExpiresAbsolute = DateTime.Now.AddSeconds(secondsInDay);
            response.Expires = secondsInDay;
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(new TimeSpan(0, 0, secondsInDay));
        }

        #endregion Public methods
    }
}