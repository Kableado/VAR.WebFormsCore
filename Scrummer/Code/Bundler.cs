using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scrummer.Code
{
    public class Bundler
    {
        #region Declarations

        private string _path = null;
        private List<string> _files = null;

        #endregion

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

        #endregion

        #region Creator

        public Bundler(string path)
        {
            _path = path;
        }

        #endregion

        #region Public methods

        public void WriteResponse(Stream outStream)
        {
            foreach (string fileName in Files)
            {
                string fileContent = File.ReadAllText(fileName);
                byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
                outStream.Write(byteArray, 0, byteArray.Length);
            }
        }

        #endregion
    }
}
