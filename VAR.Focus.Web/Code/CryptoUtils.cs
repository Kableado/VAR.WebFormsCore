using System.Security.Cryptography;
using System.Text;

namespace VAR.Focus.Web.Code
{
    public class CryptoUtils
    {
        public static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1.Create();
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static string GetRandString(int len)
        {
            byte[] bytes = new byte[len];
            var cryptoRandom = new RNGCryptoServiceProvider();
            cryptoRandom.GetBytes(bytes);

            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(bytes);
        }

        public static string GetCryptoToken()
        {
            return GetSHA1(GetRandString(10));
        }

        public static string GetHashedPassword(string password, string passwordSalt)
        {
            return GetSHA1(string.Format("{1}{0}{1}", password, passwordSalt));
        }

    }
}