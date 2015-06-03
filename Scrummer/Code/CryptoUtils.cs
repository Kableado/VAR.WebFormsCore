using System.Security.Cryptography;
using System.Text;

namespace Scrummer.Code
{
    public class CryptoUtils
    {
        public static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
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
            return CryptoUtils.GetSHA1(CryptoUtils.GetRandString(10));
        }
    }
}