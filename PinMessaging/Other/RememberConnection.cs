using System;
using PinMessaging.Model;
using System.IO.IsolatedStorage;
using PinMessaging.Utils;

namespace PinMessaging.Other
{
    class RememberConnection
    {
        private const string ConnectionInfos = "pinmessagingConnectionInfos";
        private const string FirstConnection = "pinmessagingFirstConnection";
        private const string AccessLocation = "pinmessagingAccessLocation";
        private const string AuthId = "pinmessagingAuthentificationId";

        public static void ResetAll()
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(ConnectionInfos);
                IsolatedStorageSettings.ApplicationSettings.Remove(FirstConnection);
                IsolatedStorageSettings.ApplicationSettings.Remove(AccessLocation);
                IsolatedStorageSettings.ApplicationSettings.Remove(AuthId);
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ResetAll: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        public static bool IsFirstConnection()
        {
            try
            {
                return (IsolatedStorageSettings.ApplicationSettings.Contains(FirstConnection) == false);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("IsFirstConnection: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            return true;
        }

        public static void SetFirstConnection()
        {
            try
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(FirstConnection) == true)
                    IsolatedStorageSettings.ApplicationSettings[FirstConnection] = false;
                else
                    IsolatedStorageSettings.ApplicationSettings[FirstConnection] = true;

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("SetFirstConnection: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        public static void SaveAuthId(string id)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[AuthId] = id;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("SaveAuthId: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        public static void SaveLoginPwd(PMLogInModel model)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[ConnectionInfos] = model;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("SaveLoginPwd: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        public static string GetAuthId()
        {
            try
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains(AuthId) == true
               ? (string)IsolatedStorageSettings.ApplicationSettings[AuthId]
               : null;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("GetAuthId: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            return null;
        }

        public static PMLogInModel GetLoginPwd()
        {
            try
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains(ConnectionInfos) == true
                    ? (PMLogInModel)IsolatedStorageSettings.ApplicationSettings[ConnectionInfos]
                    : null;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("GetLoginPwd: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            return null;
        }

        public static void SaveAccessLocation(bool status)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[AccessLocation] = status;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("SaveAccessLocation: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        public static bool? GetAccessLocation()
        {
            try
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains(AccessLocation) == true
                     ? (bool)IsolatedStorageSettings.ApplicationSettings[AccessLocation]
                     : (bool?)null;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("GetAccessLocation: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            return null;
        }
    }
}
