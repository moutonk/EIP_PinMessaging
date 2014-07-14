using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Phone.Info;
using Microsoft.Xna.Framework.GamerServices;

namespace PinMessaging.Utils
{
    public static class Utils
    {
        public static string GetPhoneUniqueId()
        {
            object uniqueId;

            return DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId) ? Convert.ToBase64String((byte[])uniqueId) : "";
        }

        public static double ConvertDoubleCommaToPoint(string d)
        {
            string s = d.Replace(',', '.');
            double num = 0;

            try
            {
                num = Double.Parse(s, CultureInfo.InvariantCulture);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
            }

            return num;
        }

        public static int ConvertStringToInt(string d)
        {
            int num = 0;

            try
            {
                num = int.Parse(d, CultureInfo.InvariantCulture);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
            }

            return num;
        }

        public static bool IsEmailValid(string email)
        {
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$",
                      RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int CustomMessageBox(string[] buttons, string boxTitle, string boxContent)
        {
            if (Guide.IsVisible == false)
            {
                var result = Guide.BeginShowMessageBox(
            boxTitle, boxContent, buttons, 0,
            MessageBoxIcon.None, null, null);

                result.AsyncWaitHandle.WaitOne();

                var choice = Guide.EndShowMessageBox(result);

                if (choice != null)
                    return choice.Value;
            }
        
            return -1;
        }

        public static bool PasswordSyntaxCheck(string pwd)
        {
            if (pwd.Length < 6 || pwd.Length > 20)
            {
                return false;
            }
            return true;
        }
    }
}
