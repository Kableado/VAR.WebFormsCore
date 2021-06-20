using System.IO;

namespace VAR.WebFormsCore.Code
{
    public class ServerHelpers
    {
        private static string _contentRoot = null;
        public static void SetContentRoot(string contentRoot)
        {
            _contentRoot = contentRoot;
        }

        public static string MapContentPath(string path)
        {
            string mappedPath = Path.Combine(_contentRoot, path);
            return mappedPath;
        }
    }
}
