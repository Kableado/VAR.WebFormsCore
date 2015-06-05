﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Scrummer.Code.JSON;

namespace Scrummer.Code.BusinessLogic
{
    public class Persistence
    {
        #region Private methods

        private static string GetLocalPath(string path)
        {
            string currentDir = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
            return string.Format("{0}/{1}", Directory.GetParent(currentDir), path);
        }

        #endregion

        #region public methods

        public static List<T> LoadList<T>(string file)
        {
            List<T> listResult = new List<T>();
            JSONParser parser = new JSONParser();
            parser.KnownTypes.Add(typeof(T));

            string filePath = GetLocalPath(file);
            if (File.Exists(filePath) == false) { return listResult; }

            string strJsonUsers = File.ReadAllText(filePath);
            object result = parser.Parse(strJsonUsers);

            if (result is IEnumerable<object>)
            {
                foreach (object item in (IEnumerable<object>)result)
                {
                    if (item is T)
                    {
                        listResult.Add((T)item);
                    }
                }
            }
            return listResult;
        }

        public static bool SaveList(string file, object data)
        {
            JSONWriter writter = new JSONWriter(true);
            string strJsonUsers = writter.Write(data);
            File.WriteAllText(GetLocalPath(file), strJsonUsers);
            return true;
        }

        #endregion
    }
}