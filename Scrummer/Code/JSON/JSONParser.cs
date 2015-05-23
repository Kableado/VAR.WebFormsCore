using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Scrummer.Code.JSON
{
    public class JSONParser
    {
        #region Declarations

        private ParserContext ctx;
        private bool tainted = false;

        #endregion

        #region Private methods

        private int ParseHexShort()
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                char c = ctx.Next();
                if (Char.IsDigit(c))
                {
                    value = (value << 4) | (c - '0');
                }
                else
                {
                    c = Char.ToLower(c);
                    if (c >= 'a' && c <= 'f')
                    {
                        value = (value << 4) | ((c - 'a') + 10);
                    }
                }
            }
            return value;
        }

        private String ParseQuotedString()
        {
            StringBuilder scratch = new StringBuilder();
            char c = ctx.SkipWhite();
            if (c == '"')
            {
                c = ctx.Next();
            }
            do
            {
                if (c == '\\')
                {
                    c = ctx.Next();
                    if (c == '"')
                    {
                        scratch.Append('"');
                    }
                    else if (c == '\\')
                    {
                        scratch.Append('\\');
                    }
                    else if (c == '/')
                    {
                        scratch.Append('/');
                    }
                    else if (c == 'b')
                    {
                        scratch.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        scratch.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        scratch.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        scratch.Append('\r');
                    }
                    else if (c == 't')
                    {
                        scratch.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        scratch.Append((char)ParseHexShort());
                    }
                    c = ctx.Next();
                }
                else if (c == '"')
                {
                    break;
                }
                else
                {
                    scratch.Append(c);
                    c = ctx.Next();
                }
            } while (!ctx.AtEnd());
            if (c == '"')
            {
                ctx.Next();
            }
            return scratch.ToString();
        }

        private String ParseString()
        {
            char c = ctx.SkipWhite();
            if (c == '"')
            {
                return ParseQuotedString();
            }
            StringBuilder scratch = new StringBuilder();

            while (!ctx.AtEnd()
                    && (Char.IsLetter(c) || Char.IsDigit(c) || c == '_'))
            {
                scratch.Append(c);
                c = ctx.Next();
            }

            return scratch.ToString();
        }

        private Object ParseNumber()
        {
            StringBuilder scratch = new StringBuilder();
            bool isFloat = false;
            int numberLenght = 0;
            char c;
            c = ctx.SkipWhite();

            // Sign
            if (c == '-')
            {
                scratch.Append('-');
                c = ctx.Next();
            }

            // Integer part
            while (Char.IsDigit(c))
            {
                scratch.Append(c);
                c = ctx.Next();
                numberLenght++;
            }

            // Decimal part
            if (c == '.')
            {
                isFloat = true;
                scratch.Append('.');
                c = ctx.Next();
                while (Char.IsDigit(c))
                {
                    scratch.Append(c);
                    c = ctx.Next();
                    numberLenght++;
                }
            }

            if (numberLenght == 0)
            {
                tainted = true;
                return null;
            }

            // Exponential part
            if (c == 'e' || c == 'E')
            {
                isFloat = true;
                scratch.Append('E');
                c = ctx.Next();
                if (c == '+' || c == '-')
                {
                    scratch.Append(c);
                }
                while (Char.IsDigit(c))
                {
                    scratch.Append(c);
                    c = ctx.Next();
                    numberLenght++;
                }
            }

            // Build number object from the parsed string
            String s = scratch.ToString();
            return isFloat ? (numberLenght < 17) ? (Object)Double.Parse(s)
                : Decimal.Parse(s) : (numberLenght < 19) ? (Object)System.Int32.Parse(s)
                     : (Object)System.Int32.Parse(s);
        }

        private List<object> ParseArray()
        {
            char c = ctx.SkipWhite();
            List<object> array = new List<object>();
            if (c == '[')
            {
                ctx.Next();
            }
            do
            {
                c = ctx.SkipWhite();
                if (c == ']')
                {
                    ctx.Next();
                    break;
                }
                else if (c == ',')
                {
                    ctx.Next();
                }
                else
                {
                    array.Add(ParseValue());
                }
            } while (!ctx.AtEnd());
            return array;
        }

        private Dictionary<string, object> ParseObject()
        {
            char c = ctx.SkipWhite();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            if (c == '{')
            {
                ctx.Next();
                c = ctx.SkipWhite();
            }
            String attributeName;
            Object attributeValue;
            do
            {
                attributeName = ParseString();
                c = ctx.SkipWhite();
                if (c == ':')
                {
                    ctx.Next();
                    attributeValue = ParseValue();
                    if (attributeName.Length > 0)
                    {
                        obj.Add(attributeName, attributeValue);
                    }
                }
                else if (c == ',')
                {
                    ctx.Next();
                    c = ctx.SkipWhite();
                }
                else if (c == '}')
                {
                    ctx.Next();
                    break;
                }
                else
                {
                    // Unexpected character
                    tainted = true;
                    break;
                }
            } while (!ctx.AtEnd());
            if (obj.Count == 0)
            {
                return null;
            }
            return obj;
        }

        private Object ParseValue()
        {
            Object token = null;
            char c = ctx.SkipWhite();
            switch (c)
            {
                case '"':
                    token = ParseQuotedString();
                    break;
                case '{':
                    token = ParseObject();
                    break;
                case '[':
                    token = ParseArray();
                    break;
                default:
                    if (Char.IsDigit(c) || c == '-')
                    {
                        token = ParseNumber();
                    }
                    else
                    {
                        String aux = ParseString();
                        if (aux.CompareTo("true") == 0)
                        {
                            token = true;
                        }
                        else if (aux.CompareTo("false") == 0)
                        {
                            token = false;
                        }
                        else if (aux.CompareTo("null") == 0)
                        {
                            token = null;
                        }
                        else
                        {
                            // Unexpected string
                            if (aux.Length == 0)
                            {
                                ctx.Next();
                            }
                            tainted = true;
                            token = null;
                        }
                    }
                    break;
            }
            return token;
        }

        private String CleanIdentifier(String input)
        {
            int i;
            char c;
            i = input.Length - 1;
            if (i < 0)
            {
                return input;
            }
            c = input[i];
            while (Char.IsLetter(c) || Char.IsDigit(c) || c == '_')
            {
                i--;
                if (i < 0)
                {
                    break;
                }
                c = input[i];
            }
            return input.Substring(i + 1);
        }

        #endregion

        #region Public methods

        public Object Parse(String text)
        {
            // Get the first object
            ctx = new ParserContext(text);
            tainted = false;
            ctx.Mark();
            Object obj = ParseValue();
            if (ctx.AtEnd())
            {
                return obj;
            }

            // "But wait, there is more!"
            int idx = 0;
            String name = "";
            String strInvalidPrev = "";
            Dictionary<string, object> superObject = new Dictionary<string, object>();
            do
            {
                // Add the object to the superObject
                if (!tainted && name.Length > 0 && obj != null)
                {
                    if (name.Length == 0)
                    {
                        name = String.Format("{0:D2}", idx);
                    }
                    superObject.Add(name, obj);
                    idx++;
                    name = "";
                }
                else
                {
                    String strInvalid = ctx.GetMarked();
                    strInvalid = strInvalid.Trim();
                    if (strInvalidPrev.Length > 0
                            && "=".CompareTo(strInvalid) == 0)
                    {
                        name = CleanIdentifier(strInvalidPrev);
                    }
                    else
                    {
                        name = "";
                    }
                    strInvalidPrev = strInvalid;
                }

                // Check end
                if (ctx.AtEnd())
                {
                    break;
                }

                // Get next object
                tainted = false;
                ctx.Mark();
                obj = ParseValue();

            } while (true);
            return superObject;
        }

        #endregion
    }
}