using System;
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
    }
}
