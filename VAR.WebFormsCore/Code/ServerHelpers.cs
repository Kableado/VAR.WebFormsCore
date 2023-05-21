using System.Globalization;
using System.Text;

namespace VAR.WebFormsCore.Code
{
    public static class ServerHelpers
    {
        private static string _contentRoot;
        public static void SetContentRoot(string contentRoot) { _contentRoot = contentRoot; }

        public static string MapContentPath(string path)
        {
            string mappedPath = string.Concat(_contentRoot, "/", path);
            return mappedPath;
        }

        public static string HtmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) { return text; }

            StringBuilder sbResult = new();

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == '<') { sbResult.Append("&lt;"); }
                else if (ch == '>') { sbResult.Append("&gt;"); }
                else if (ch == '"') { sbResult.Append("&quot;"); }
                else if (ch == '\'') { sbResult.Append("&#39;"); }
                else if (ch == '&') { sbResult.Append("&amp;"); }
                else if (ch > 127)
                {
                    sbResult.Append("&#");
                    sbResult.Append(((int) ch).ToString(NumberFormatInfo.InvariantInfo));
                    sbResult.Append(';');
                }
                else { sbResult.Append(ch); }
            }

            return sbResult.ToString();
        }

        public static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) { return text; }

            StringBuilder sbResult = new();

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == ' ') { sbResult.Append('+'); }
                else if (IsUrlSafe(ch) == false) { sbResult.AppendFormat("%{0:X02}", ch); }
                else { sbResult.Append(ch); }
            }

            return sbResult.ToString();
        }

        public static bool IsUrlSafe(char ch)
        {
            if (
                (ch >= 'a' && ch <= 'z') ||
                (ch >= 'A' && ch <= 'Z') ||
                (ch >= '0' && ch <= '9') ||
                ch == '-' ||
                ch == '_' ||
                ch == '.' ||
                ch == '!' ||
                ch == '*' ||
                ch == '(' ||
                ch == ')') { return true; }

            return false;
        }
    }
}