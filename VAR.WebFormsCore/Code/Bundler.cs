using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace VAR.WebFormsCore.Code
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

        private static Encoding _utf8Econding = new UTF8Encoding();

        public async void WriteResponse(HttpResponse response, string contentType)
        {
            StringWriter textWriter = new StringWriter();
            response.ContentType = contentType;
            foreach (string fileName in AssemblyFiles)
            {
                Stream resourceStream = _assembly.GetManifestResourceStream(fileName);
                string fileContent = new StreamReader(resourceStream).ReadToEnd();
                textWriter.Write(fileContent);
                textWriter.Write("\n\n");
            }
            foreach (string fileName in AbsoluteFiles)
            {
                string fileContent = File.ReadAllText(fileName);
                textWriter.Write(fileContent);
                textWriter.Write("\n\n");
            }
            byte[] byteObject = _utf8Econding.GetBytes(textWriter.ToString());
            await response.Body.WriteAsync(byteObject);
        }

        #endregion Public methods
    }
}