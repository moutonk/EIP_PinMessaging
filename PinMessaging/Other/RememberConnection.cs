using PinMessaging.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinMessaging.Utils;
using System.Windows;

namespace PinMessaging.Other
{
    class RememberConnection
    {
        public class ConnectionData
        {
            public ConnectionData(string email, string pwd)
            {
                Email = email;
                Password = pwd;
            }

            public string Email { get; set; }
            public string Password { get; set; }
        }

        private static readonly string connectionInfos = "pinmessagingConnectionInfos";
        private static readonly string firstConnection = "pinmessagingFirstConnection";

        public static void ResetAll()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(connectionInfos);
            IsolatedStorageSettings.ApplicationSettings.Remove(firstConnection);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsFirstConnection()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(firstConnection) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SetFirstConnection()
        {
            IsolatedStorageSettings.ApplicationSettings[firstConnection] = true;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void SaveLoginPwd(PMLogInModel model)
        {
            IsolatedStorageSettings.ApplicationSettings[connectionInfos] = model;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static PMLogInModel GetLoginPwd()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(connectionInfos) == true)
                return (PMLogInModel)IsolatedStorageSettings.ApplicationSettings[connectionInfos];
            return null;
        }
    }
}
