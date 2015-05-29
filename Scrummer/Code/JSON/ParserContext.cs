using System;

namespace Scrummer.Code.JSON
{
    public class ParserContext
    {
        #region Declarations

        private String text;
        private int length;
        private int i;
        private int markStart;

        #endregion

        #region Creator

        public ParserContext(String text)
        {
            this.text = text;
            this.length = text.Length;
            this.i = 0;
            this.markStart = 0;
        }

        #endregion

        #region Public methods

        public char SkipWhite()
        {
            while (i < length && Char.IsWhiteSpace(text[i]))
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
            markStart = this.i;
        }

        public String GetMarked()
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
