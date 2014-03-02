using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinMessaging.Utils
{
    public class Utils
    {
        public static bool PasswordSyntaxCheck(string pwd)
        {
            if (pwd.Length < 4 || pwd.Length > 20)
            {
                return false;
            }
            return true;
        }
    }
}
