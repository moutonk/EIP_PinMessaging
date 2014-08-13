using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Phone.Info;
using Microsoft.Xna.Framework.GamerServices;
using PinMessaging.Model;
using PinMessaging.Resources;

namespace PinMessaging.Utils
{
    public static class Utils
    {
        public static PMPinModel.PinsType PinToPinType(PMPinModel pin)
        {
            if (pin.Private == true)
                return pin.PinType + 6;
            return pin.PinType;
        }

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

        public static int? ConvertStringToInt(string d)
        {
            int? num = 0;

            try
            {
                num = Int32.Parse(d, CultureInfo.InvariantCulture);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
            }

            return num;
        }

        public static double? ConvertStringToDouble(string d)
        {
            double? num = 0;

            try
            {
                num = Double.Parse(d, CultureInfo.InvariantCulture);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ConvertStringToDouble: " + d, exp, Logs.Error.ErrorsPriority.NotCritical);
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

        public static DateTime ConvertFromUnixTimestamp(double? timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return timestamp == null ? origin : origin.AddMilliseconds((double)timestamp);
        }

        public static Tuple<string, string, Uri> GetGradeInfo(PMGradeModel.GradeType grade)
        {
            switch (grade)
            {
                case PMGradeModel.GradeType.PointBronze:
                    return new Tuple<string, string, Uri>(AppResources.GradePointCopper, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.PointArgent:
                    return new Tuple<string, string, Uri>(AppResources.GradePointSilver, AppResources.BadgePointSilver, null);

                case PMGradeModel.GradeType.PointOr:
                    return new Tuple<string, string, Uri>(AppResources.GradePointGold, AppResources.BadgePointGold, null);

                case PMGradeModel.GradeType.Pin50:
                    return new Tuple<string, string, Uri>(AppResources.GradePin50, AppResources.BadgePin50, null);

                case PMGradeModel.GradeType.Message50:
                    return new Tuple<string, string, Uri>(AppResources.GradeMessage50, AppResources.BadgeComments50, null);

                case PMGradeModel.GradeType.Betatester:
                    return new Tuple<string, string, Uri>(AppResources.GradeBetaTester, AppResources.BadgeBetatester, null);
            }
            return null;
        }
    }
}
