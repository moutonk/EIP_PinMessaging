using System;
using System.Globalization;
using Microsoft.Phone.Info;

namespace PinMessaging.Utils
{
    class Phone
    {
        public static string GetPhoneUniqueId()
        { 
            object uniqueId;

            return DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId) ? Convert.ToBase64String((byte[]) uniqueId) : "";
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
    }
}
