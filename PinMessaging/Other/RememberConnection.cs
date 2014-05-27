using System;
using PinMessaging.Model;
using System.IO.IsolatedStorage;
using PinMessaging.Utils;

namespace PinMessaging.Other
{
    class RememberConnection
    {
        public class ConnectionData
        {
            //public ConnectionData(string email, string pwd)
            //{
          //      Email = email;
            //    Password = pwd;
            //}

            //public string Email { get; set; }
            //public string Password { get; set; }
        }

        private const string ConnectionInfos = "pinmessagingConnectionInfos";
        private const string FirstConnection = "pinmessagingFirstConnection";
        private const string AccessLocation = "pinmessagingAccessLocation";
        private const string AuthId = "pinmessagingAuthentificationId";

        public static void ResetAll()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(ConnectionInfos);
            IsolatedStorageSettings.ApplicationSettings.Remove(FirstConnection);
            IsolatedStorageSettings.ApplicationSettings.Remove(AccessLocation);
            IsolatedStorageSettings.ApplicationSettings.Remove(AuthId);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsFirstConnection()
        {
            return (IsolatedStorageSettings.ApplicationSettings.Contains(FirstConnection) == false);
        }

        public static void SetFirstConnection()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(FirstConnection) == true)
            {
                IsolatedStorageSettings.ApplicationSettings[FirstConnection] = false;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[FirstConnection] = true;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void SaveAuthId(string id)
        {
            IsolatedStorageSettings.ApplicationSettings[AuthId] = id;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void SaveLoginPwd(PMLogInModel model)
        {
            IsolatedStorageSettings.ApplicationSettings[ConnectionInfos] = model;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static string GetAuthId()
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(AuthId) == true
                ? (string)IsolatedStorageSettings.ApplicationSettings[AuthId]
                : null;
        }

        public static PMLogInModel GetLoginPwd()
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(ConnectionInfos) == true
                ? (PMLogInModel)IsolatedStorageSettings.ApplicationSettings[ConnectionInfos]
                : null;
        }

        public static void SaveAccessLocation(bool status)
        {
            IsolatedStorageSettings.ApplicationSettings[AccessLocation] = status;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool? GetAccessLocation()
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(AccessLocation) == true
                ? (bool)IsolatedStorageSettings.ApplicationSettings[AccessLocation]
                : (bool?)null;
        }
    }
}
