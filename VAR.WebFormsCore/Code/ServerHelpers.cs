using System.Globalization;
using System.Text;

namespace VAR.WebFormsCore.Code;

public static class ServerHelpers
{
    private static string? _contentRoot;

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

        foreach (var ch in text)
        {
            switch (ch)
            {
                case '<':
                    sbResult.Append("&lt;");
                    break;
                case '>':
                    sbResult.Append("&gt;");
                    break;
                case '"':
                    sbResult.Append("&quot;");
                    break;
                case '\'':
                    sbResult.Append("&#39;");
                    break;
                case '&':
                    sbResult.Append("&amp;");
                    break;
                default:
                {
                    if (ch > 127)
                    {
                        sbResult.Append("&#");
                        sbResult.Append(((int)ch).ToString(NumberFormatInfo.InvariantInfo));
                        sbResult.Append(';');
                    }
                    else { sbResult.Append(ch); }

                    break;
                }
            }
        }

        return sbResult.ToString();
    }

    public static string UrlEncode(string text)
    {
        if (string.IsNullOrEmpty(text)) { return text; }

        StringBuilder sbResult = new();

        foreach (var ch in text)
        {
            if (ch == ' ') { sbResult.Append('+'); }
            else if (IsUrlSafe(ch) == false)
            {
                int intCh = ch;
                sbResult.Append($"%{intCh:X02}");
            }
            else { sbResult.Append(ch); }
        }

        return sbResult.ToString();
    }

    private static bool IsUrlSafe(char ch)
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