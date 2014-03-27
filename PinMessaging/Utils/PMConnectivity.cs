using System;
using Microsoft.Phone.Net.NetworkInformation;

namespace PinMessaging.Utils
{
    static class PMConnectivity
    {
        static PMConnectivity()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(ChangeDetected);
        }

        public static  void ShowConnectivity()
        {
            Logs.Output.ShowOutput("IsCellularDataEnabled ? " + ((DeviceNetworkInformation.IsCellularDataEnabled) ? "Yes" : "No"));
            Logs.Output.ShowOutput("IsNetworkAvailable ? " + ((DeviceNetworkInformation.IsNetworkAvailable) ? "Yes" : "No"));
            Logs.Output.ShowOutput("IsWiFiEnabled ? " + ((DeviceNetworkInformation.IsWiFiEnabled) ? "Yes" : "No"));
        }

        static void ChangeDetected(object sender, NetworkNotificationEventArgs e)
        {
            string change = string.Empty;

            switch (e.NotificationType)
            {
                case NetworkNotificationType.InterfaceConnected:
                    change = "Connected to ";
                    break;
                case NetworkNotificationType.InterfaceDisconnected:
                    change = "Disconnected from ";
                    break;
                case NetworkNotificationType.CharacteristicUpdate:
                    change = "Characteristics changed for ";
                    break;
                default:
                    change = "Unknown change with ";
                    break;
            }

            string changeInformation = String.Format(" {0} {1} {2} ({3})",
                        DateTime.Now.ToString(), change, e.NetworkInterface.InterfaceName,
                        e.NetworkInterface.InterfaceType.ToString());

            Logs.Output.ShowOutput(changeInformation);
        }
    }
}
