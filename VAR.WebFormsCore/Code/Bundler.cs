using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace VAR.WebForms.Common.Code
{
    public class Bundler
    {
        #region Declarations

        private Assembly _assembly = null;
        private string _assemblyNamespace = null;
        private List<string> _assemblyFiles = null;
        private string _absolutePath = null;
        private List<string> _absoluteFiles = null;

        #endregion Declarations

        #region Properties

        private List<string> AssemblyFiles
        {
            get
            {
                if (_assemblyFiles != null) { return _assemblyFiles; }
                if (_assembly == null || string.IsNullOrEmpty(_assemblyNamespace))
                {
                    _assemblyFiles = new List<string>();
                    return _assemblyFiles;
                }
                string assemblyPath = string.Concat(_assembly.GetName().Name, ".", _assemblyNamespace, ".");
                _assemblyFiles = _assembly.GetManifestResourceNames().Where(r => r.StartsWith(assemblyPath)).ToList();
                return _assemblyFiles;
            }
        }

        private List<string> AbsoluteFiles
        {
            get
            {
                if (_absoluteFiles != null) { return _absoluteFiles; }

                if (string.IsNullOrEmpty(_absolutePath))
                {
                    _absoluteFiles = new List<string>();
                    return _absoluteFiles;
                }
                DirectoryInfo dir = new DirectoryInfo(_absolutePath);
                FileInfo[] files = dir.GetFiles();
                _absoluteFiles = files.OrderBy(file => file.FullName).Select(file2 => file2.FullName).ToList();
                return _absoluteFiles;
            }
        }

        #endregion Properties

        #region Creator

        public Bundler(Assembly assembly = null, string assemblyNamespace = null, string absolutePath = null)
        {
            _assembly = assembly;
            _assemblyNamespace = assemblyNamespace;
            _absolutePath = absolutePath;
        }

        #endregion Creator

        #region Public methods

        public void WriteResponse(HttpResponse response, string contentType)
        {
            response.ContentType = contentType;
            foreach (string fileName in AssemblyFiles)
            {
                Stream resourceStream = _assembly.GetManifestResourceStream(fileName);
                string fileContent = new StreamReader(resourceStream).ReadToEnd();
                byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
                if (byteArray.Length > 0)
                {
                    response.Body.Write(byteArray, 0, byteArray.Length);

                    byteArray = Encoding.UTF8.GetBytes("\n\n");
                    response.Body.Write(byteArray, 0, byteArray.Length);
                }
            }
            foreach (string fileName in AbsoluteFiles)
            {
                string fileContent = File.ReadAllText(fileName);
                byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
                if (byteArray.Length > 0)
                {
                    response.Body.Write(byteArray, 0, byteArray.Length);

                    byteArray = Encoding.UTF8.GetBytes("\n\n");
                    response.Body.Write(byteArray, 0, byteArray.Length);
                }
            }
        }

        #endregion Public methods
    }
}