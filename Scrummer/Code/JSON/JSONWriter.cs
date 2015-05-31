using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scrummer.Code.JSON
{
    public class JSONWriter
    {
        #region Declarations

        private bool indent = false;
        private bool useTabForIndent = false;
        private int indentChars = 4;
        private int indentThresold = 3;

        #endregion

        #region Creator

        public JSONWriter() { }

        public JSONWriter(int indentChars)
        {
            this.indent = true;
            this.indentChars = indentChars;
            this.useTabForIndent = false;
        }

        public JSONWriter(bool useTabForIndent)
        {
            this.indent = true;
            this.useTabForIndent = useTabForIndent;
        }

        #endregion

        #region Private methods

        private bool IsValue(Object obj)
        {
            if (obj == null)
            {
                return true;
            }
            if ((obj is float) || (obj is double) ||
                (obj is System.Int16) || (obj is System.Int32) || (obj is System.Int64)
                    || (obj is String) || (obj is Boolean))
            {
                return true;
            }
            return false;
        }

        private void WriteIndent(StringBuilder sbOutput, int level)
        {
            if (!indent)
            {
                return;
            }
            sbOutput.Append('\n');
            if (useTabForIndent)
            {
                for (int i = 0; i < level; i++) { sbOutput.Append('\t'); }
            }
            else
            {
                int n = level * indentChars;
                for (int i = 0; i < n; i++) { sbOutput.Append(' '); }
            }
        }

        private void WriteString(StringBuilder sbOutput, String str)
        {
            sbOutput.Append('"');
            char c;
            int n = str.Length;
            for (int i = 0; i < n; i++)
            {
                c = str[i];
                if (c == '"') { sbOutput.Append("\\\""); }
                else if (c == '\\') { sbOutput.Append("\\\\"); }
                else if (c == '/') { sbOutput.Append("\\/"); }
                else if (c == '\b') { sbOutput.Append("\\b"); }
                else if (c == '\f') { sbOutput.Append("\\f"); }
                else if (c == '\n') { sbOutput.Append("\\n"); }
                else if (c == '\r') { sbOutput.Append("\\r"); }
                else if (c == '\t') { sbOutput.Append("\\t"); }
                else { sbOutput.Append(c); }
                // FIXME: Unicode characters
            }
            sbOutput.Append('"');
        }

        private void WriteValue(StringBuilder sbOutput, Object obj, int level, bool useReflection)
        {
            if (obj == null || obj is DBNull)
            {
                // NULL
                sbOutput.Append("null");
            }
            else if ((obj is float) || (obj is double) ||
              (obj is System.Int16) || (obj is System.Int32) || (obj is System.Int64))
            {
                // Numbers
                sbOutput.Append(obj.ToString());
            }
            else if (obj is String)
            {
                // Strings
                WriteString(sbOutput, (String)obj);
            }
            else if (obj is Boolean)
            {
                // Booleans
                sbOutput.Append(((Boolean)obj) ? "true" : "false");
            }
            else if (obj is DateTime)
            {
                // DateTime
                sbOutput.Append('"');
                sbOutput.Append(((DateTime)obj).ToString("yyyy-MM-ddTHH:mm:ssZ"));
                sbOutput.Append('"');
            }
            else if (obj is IDictionary)
            {
                // Objects
                WriteObject(sbOutput, obj, level);
            }
            else if (obj is IEnumerable)
            {
                // Array/List
                WriteList(sbOutput, obj, level);
            }
            else
            {
                if (useReflection)
                {
                    // Reflected object
                    WriteReflectedObject(sbOutput, obj, level);
                }
                else
                {
                    WriteString(sbOutput, Convert.ToString(obj));
                }
            }
        }

        private void WriteList(StringBuilder sbOutput, Object obj, int level)
        {
            IEnumerable list = (IEnumerable)obj;
            int n = 0;

            // Check if it is a leaf object
            bool isLeaf = true;
            foreach (object childObj in list)
            {
                if (!IsValue(childObj))
                {
                    isLeaf = false;
                }
                n++;
            }

            // Empty
            if (n == 0)
            {
                sbOutput.Append("[ ]");
                return;
            }

            // Write array
            bool first = true;
            sbOutput.Append("[ ");
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level + 1);
            }
            foreach (object childObj in list)
            {
                if (!first)
                {
                    sbOutput.Append(", ");
                    if (!isLeaf || n > indentThresold)
                    {
                        WriteIndent(sbOutput, level + 1);
                    }
                }
                first = false;
                WriteValue(sbOutput, childObj, level + 1, true);
            }
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level);
            }
            sbOutput.Append(" ]");
        }

        private void WriteObject(StringBuilder sbOutput, Object obj, int level)
        {
            IDictionary map = (IDictionary)obj;
            int n = map.Count;

            // Empty
            if (map.Count == 0)
            {
                sbOutput.Append("{ }");
                return;
            }

            // Check if it is a leaf object
            bool isLeaf = true;
            foreach (object value in map.Values)
            {
                if (!IsValue(value))
                {
                    isLeaf = false;
                    break;
                }
            }

            // Write object
            bool first = true;
            sbOutput.Append("{ ");
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level + 1);
            }
            foreach (object key in map.Keys)
            {
                object value = map[key];
                if (!first)
                {
                    sbOutput.Append(", ");
                    if (!isLeaf || n > indentThresold)
                    {
                        WriteIndent(sbOutput, level + 1);
                    }
                }
                first = false;
                WriteString(sbOutput, Convert.ToString(key));
                sbOutput.Append(": ");
                WriteValue(sbOutput, value, level + 1, true);
            }
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level);
            }
            sbOutput.Append(" }");
        }

        private void WriteReflectedObject(StringBuilder sbOutput, Object obj, int level)
        {
            Type type = obj.GetType();
            PropertyInfo[] rawProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (PropertyInfo property in rawProperties)
            {
                if (property.CanRead)
                {
                    properties.Add(property);
                }
            }
            int n = properties.Count;

            // Empty
            if (n == 0)
            {
                sbOutput.Append("{ }");
                return;
            }

            // Check if it is a leaf object
            bool isLeaf = true;
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj, null);
                if (!IsValue(value))
                {
                    isLeaf = false;
                    break;
                }
            }

            // Write object
            bool first = true;
            sbOutput.Append("{ ");
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level + 1);
            }
            foreach (PropertyInfo property in properties)
            {
                object value = null;
                MethodInfo getMethod = property.GetGetMethod();
                ParameterInfo[] parameters = getMethod.GetParameters();
                if (parameters.Length == 0)
                {
                    value = property.GetValue(obj, null);
                }
                if (!first)
                {
                    sbOutput.Append(", ");
                    if (!isLeaf || n > indentThresold)
                    {
                        WriteIndent(sbOutput, level + 1);
                    }
                }
                first = false;
                WriteString(sbOutput, property.Name);
                sbOutput.Append(": ");
                WriteValue(sbOutput, value, level + 1, false);
            }
            if (!isLeaf || n > indentThresold)
            {
                WriteIndent(sbOutput, level);
            }
            sbOutput.Append(" }");
        }

        #endregion

        #region Public methods

        public String Write(Object obj)
        {
            StringBuilder sbOutput = new StringBuilder();
            WriteValue(sbOutput, obj, 0, true);
            return sbOutput.ToString();
        }

        #endregion
    }
}
