
using System.Security.Cryptography;
using System.Text;

namespace PinMessaging.Utils
{
    class Encrypt
    {
        public static string ConvertToSHA1(string toConvert)
        {
            SHA1 sha1 = new SHA1Managed();
            var returnValue = new StringBuilder();

            var hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(toConvert));

            foreach (var t in hashData)
            {
                returnValue.Append(t.ToString());
            }
            return returnValue.ToString();
        }
    }
}
