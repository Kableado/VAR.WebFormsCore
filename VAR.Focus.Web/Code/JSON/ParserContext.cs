using System;

namespace VAR.Focus.Web.Code.JSON
{
    public class ParserContext
    {
        #region Declarations

        private string text;
        private int length;
        private int i;
        private int markStart;

        #endregion

        #region Creator

        public ParserContext(string text)
        {
            this.text = text;
            length = text.Length;
            i = 0;
            markStart = 0;
        }

        #endregion

        #region Public methods

        public char SkipWhite()
        {
            while (i < length && char.IsWhiteSpace(text[i]))
            {
                i++;
            }
            if (AtEnd())
            {
                return (char)0;
            }
            return text[i];
        }

        public char Next()
        {
            i++;
            if (AtEnd())
            {
                return (char)0;
            }
            return text[i];
        }

        public bool AtEnd()
        {
            return i >= length;
        }

        public void Mark()
        {
            markStart = i;
        }

        public string GetMarked()
        {
            if (i < length && markStart < length)
            {
                return text.Substring(markStart, i);
            }
            else
            {
                if (markStart < length)
                {
                    return text.Substring(markStart, length);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion
    }
}
