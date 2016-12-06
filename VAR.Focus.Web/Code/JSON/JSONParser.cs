using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace VAR.Focus.Web.Code.JSON
{
    public class JSONParser
    {
        #region Declarations

        private ParserContext _ctx;
        private bool _tainted = false;

        private List<Type> _knownTypes = new List<Type>();

        #endregion

        #region Properties

        public bool Tainted
        {
            get { return _tainted; }
        }

        public List<Type> KnownTypes
        {
            get { return _knownTypes; }
        }

        #endregion

        #region Private methods

        private static Dictionary<Type, PropertyInfo[]> _dictProperties = new Dictionary<Type, PropertyInfo[]>();

        private PropertyInfo[] Type_GetProperties(Type type)
        {
            PropertyInfo[] typeProperties = null;
            if (_dictProperties.ContainsKey(type)) { typeProperties = _dictProperties[type]; }
            else
            {
                lock(_dictProperties){

                    if (_dictProperties.ContainsKey(type)) { typeProperties = _dictProperties[type]; }
                    else
                    {
                        typeProperties = type.GetProperties(BindingFlags.Public | BindingFlags.OptionalParamBinding | BindingFlags.Instance);
                        _dictProperties.Add(type, typeProperties);
                    }
                }
            }
            return typeProperties;
        }

        private float CompareToType(Dictionary<string, object> obj, Type type)
        {
            PropertyInfo[] typeProperties = Type_GetProperties(type);
            int count = 0;
            foreach (PropertyInfo prop in typeProperties)
            {
                if (obj.ContainsKey(prop.Name))
                {
                    count++;
                }
            }
            return ((float)count / typeProperties.Length);
        }

        private object ConvertToType(Dictionary<string, object> obj, Type type)
        {
            PropertyInfo[] typeProperties = Type_GetProperties(type);
            object newObj = ObjectActivator.CreateInstance(type);
            foreach (PropertyInfo prop in typeProperties)
            {
                if (obj.ContainsKey(prop.Name))
                {
                    prop.SetValue(newObj, Convert.ChangeType(obj[prop.Name], prop.PropertyType), null);
                }
            }
            return newObj;
        }

        private object TryConvertToTypes(Dictionary<string, object> obj)
        {
            Type bestMatch = null;
            float bestMatchFactor = 0.0f;
            foreach (Type type in _knownTypes)
            {
                float matchFactor = CompareToType(obj, type);
                if (matchFactor > bestMatchFactor)
                {
                    bestMatch = type;
                    bestMatchFactor = matchFactor;
                }
            }
            if (bestMatch != null)
            {
                return ConvertToType(obj, bestMatch);
            }
            return obj;
        }

        private int ParseHexShort()
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                char c = _ctx.Next();
                if (char.IsDigit(c))
                {
                    value = (value << 4) | (c - '0');
                }
                else
                {
                    c = char.ToLower(c);
                    if (c >= 'a' && c <= 'f')
                    {
                        value = (value << 4) | ((c - 'a') + 10);
                    }
                }
            }
            return value;
        }

        private string ParseQuotedString()
        {
            StringBuilder scratch = new StringBuilder();
            char c = _ctx.SkipWhite();
            if (c == '"')
            {
                c = _ctx.Next();
            }
            do
            {
                if (c == '\\')
                {
                    c = _ctx.Next();
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
                    c = _ctx.Next();
                }
                else if (c == '"')
                {
                    break;
                }
                else
                {
                    scratch.Append(c);
                    c = _ctx.Next();
                }
            } while (!_ctx.AtEnd());
            if (c == '"')
            {
                _ctx.Next();
            }
            return scratch.ToString();
        }

        private string ParseString()
        {
            char c = _ctx.SkipWhite();
            if (c == '"')
            {
                return ParseQuotedString();
            }
            StringBuilder scratch = new StringBuilder();

            while (!_ctx.AtEnd()
                    && (char.IsLetter(c) || char.IsDigit(c) || c == '_'))
            {
                scratch.Append(c);
                c = _ctx.Next();
            }

            return scratch.ToString();
        }

        private object ParseNumber()
        {
            StringBuilder scratch = new StringBuilder();
            bool isFloat = false;
            int numberLenght = 0;
            char c;
            c = _ctx.SkipWhite();

            // Sign
            if (c == '-')
            {
                scratch.Append('-');
                c = _ctx.Next();
            }

            // Integer part
            while (char.IsDigit(c))
            {
                scratch.Append(c);
                c = _ctx.Next();
                numberLenght++;
            }

            // Decimal part
            if (c == '.')
            {
                isFloat = true;
                scratch.Append('.');
                c = _ctx.Next();
                while (char.IsDigit(c))
                {
                    scratch.Append(c);
                    c = _ctx.Next();
                    numberLenght++;
                }
            }

            if (numberLenght == 0)
            {
                _tainted = true;
                return null;
            }

            // Exponential part
            if (c == 'e' || c == 'E')
            {
                isFloat = true;
                scratch.Append('E');
                c = _ctx.Next();
                if (c == '+' || c == '-')
                {
                    scratch.Append(c);
                }
                while (char.IsDigit(c))
                {
                    scratch.Append(c);
                    c = _ctx.Next();
                    numberLenght++;
                }
            }

            // Build number object from the parsed string
            string s = scratch.ToString();
            return isFloat ? (numberLenght < 17) ? (object)double.Parse(s)
                : decimal.Parse(s) : (numberLenght < 19) ? int.Parse(s)
                     : (object)int.Parse(s);
        }

        private List<object> ParseArray()
        {
            char c = _ctx.SkipWhite();
            List<object> array = new List<object>();
            if (c == '[')
            {
                _ctx.Next();
            }
            do
            {
                c = _ctx.SkipWhite();
                if (c == ']')
                {
                    _ctx.Next();
                    break;
                }
                else if (c == ',')
                {
                    _ctx.Next();
                }
                else
                {
                    array.Add(ParseValue());
                }
            } while (!_ctx.AtEnd());
            return array;
        }

        private Dictionary<string, object> ParseObject()
        {
            char c = _ctx.SkipWhite();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            if (c == '{')
            {
                _ctx.Next();
                c = _ctx.SkipWhite();
            }
            string attributeName;
            object attributeValue;
            do
            {
                attributeName = ParseString();
                c = _ctx.SkipWhite();
                if (c == ':')
                {
                    _ctx.Next();
                    attributeValue = ParseValue();
                    if (attributeName.Length > 0)
                    {
                        obj.Add(attributeName, attributeValue);
                    }
                }
                else if (c == ',')
                {
                    _ctx.Next();
                    c = _ctx.SkipWhite();
                }
                else if (c == '}')
                {
                    _ctx.Next();
                    break;
                }
                else
                {
                    // Unexpected character
                    _tainted = true;
                    break;
                }
            } while (!_ctx.AtEnd());
            if (obj.Count == 0)
            {
                return null;
            }

            return obj;
        }

        private object ParseValue()
        {
            object token = null;
            char c = _ctx.SkipWhite();
            switch (c)
            {
                case '"':
                    token = ParseQuotedString();
                    break;
                case '{':
                    Dictionary<string, object> obj = ParseObject();
                    token = TryConvertToTypes(obj);
                    break;
                case '[':
                    token = ParseArray();
                    break;
                default:
                    if (char.IsDigit(c) || c == '-')
                    {
                        token = ParseNumber();
                    }
                    else
                    {
                        string aux = ParseString();
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
                                _ctx.Next();
                            }
                            _tainted = true;
                            token = null;
                        }
                    }
                    break;
            }
            return token;
        }

        private string CleanIdentifier(string input)
        {
            int i;
            char c;
            i = input.Length - 1;
            if (i < 0)
            {
                return input;
            }
            c = input[i];
            while (char.IsLetter(c) || char.IsDigit(c) || c == '_')
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

        public object Parse(string text)
        {
            // Get the first object
            _ctx = new ParserContext(text);
            _tainted = false;
            _ctx.Mark();
            object obj = ParseValue();
            if (_ctx.AtEnd())
            {
                return obj;
            }

            // "But wait, there is more!"
            int idx = 0;
            string name = "";
            string strInvalidPrev = "";
            Dictionary<string, object> superObject = new Dictionary<string, object>();
            do
            {
                // Add the object to the superObject
                if (!_tainted && name.Length > 0 && obj != null)
                {
                    if (name.Length == 0)
                    {
                        name = string.Format("{0:D2}", idx);
                    }
                    superObject.Add(name, obj);
                    idx++;
                    name = "";
                }
                else
                {
                    string strInvalid = _ctx.GetMarked();
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
                if (_ctx.AtEnd())
                {
                    break;
                }

                // Get next object
                _tainted = false;
                _ctx.Mark();
                obj = ParseValue();

            } while (true);
            return superObject;
        }

        #endregion
    }
}
