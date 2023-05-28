using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VAR.WebFormsCore.Code;

public class Bundler
{
    #region Declarations

    private readonly Assembly? _assembly;
    private readonly string? _assemblyNamespace;
    private List<string>? _assemblyFiles;
    private readonly string? _absolutePath;
    private List<string>? _absoluteFiles;

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

            if (_absolutePath == null || string.IsNullOrEmpty(_absolutePath))
            {
                _absoluteFiles = new List<string>();
                return _absoluteFiles;
            }

            if (Directory.Exists(_absolutePath) == false)
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

    public Bundler(Assembly? assembly = null, string? assemblyNamespace = null, string? absolutePath = null)
    {
        _assembly = assembly;
        _assemblyNamespace = assemblyNamespace;
        _absolutePath = absolutePath;
    }

    #endregion Creator

    #region Public methods

    private static readonly Encoding Utf8Encoding = new UTF8Encoding();

    public void WriteResponse(IWebContext context, string contentType)
    {
        StringWriter textWriter = new StringWriter();
        context.ResponseContentType = contentType;
        foreach (string fileName in AssemblyFiles)
        {
            Stream? resourceStream = _assembly?.GetManifestResourceStream(fileName);
            if (resourceStream != null)
            {
                string fileContent = new StreamReader(resourceStream).ReadToEnd();
                textWriter.Write(fileContent);
            }

            textWriter.Write("\n\n");
        }

        foreach (string fileName in AbsoluteFiles)
        {
            string fileContent = File.ReadAllText(fileName);
            textWriter.Write(fileContent);
            textWriter.Write("\n\n");
        }

        byte[] byteObject = Utf8Encoding.GetBytes(textWriter.ToString());
        context.ResponseWriteBin(byteObject);
    }

    #endregion Public methods
}