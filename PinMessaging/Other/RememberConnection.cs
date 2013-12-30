using PinMessaging.Model;
using System.IO.IsolatedStorage;

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

        private const string ConnectionInfos = "pinmessagingConnectionInfos";
        private const string FirstConnection = "pinmessagingFirstConnection";

        public static void ResetAll()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(ConnectionInfos);
            IsolatedStorageSettings.ApplicationSettings.Remove(FirstConnection);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool IsFirstConnection()
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(FirstConnection) == false;
        }

        public static void SetFirstConnection()
        {
            IsolatedStorageSettings.ApplicationSettings[FirstConnection] = true;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void SaveLoginPwd(PMLogInModel model)
        {
            IsolatedStorageSettings.ApplicationSettings[ConnectionInfos] = model;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static PMLogInModel GetLoginPwd()
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(ConnectionInfos) == true
                ? (PMLogInModel) IsolatedStorageSettings.ApplicationSettings[ConnectionInfos]
                : null;
        }
    }
}
