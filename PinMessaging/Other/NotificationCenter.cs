using System;
using System.Text;
using Windows.UI.Core;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Tasks;
using PinMessaging.Utils;
using PinMessaging.View;

namespace PinMessaging.Other
{
    public static class NotificationCenter
    {
        enum NotificationType
        {
            NewComment = 0,
            PinModified,
            NewGrade,
            NewFavorite
        }

        /// Holds the push channel that is created or found.
        private static readonly HttpNotificationChannel PushChannel;

        public static string PushChannelUri { get; set; }
        private const string ChannelName = "ToastChannel";
        private static PMMapView _map = null;

        public static void Init(PMMapView map)
        {
            _map = map;
        }

        //call static constructor
        public static void Start() {}

        static NotificationCenter()
        {
            Logs.Output.ShowOutput("NotificationCenter constructor begin");

            // Try to find the push channel.
            PushChannel = HttpNotificationChannel.Find(ChannelName);

            // If the channel was not found, then create a new connection to the push service.
            if (PushChannel == null)
            {
                PushChannel = new HttpNotificationChannel(ChannelName);

                // Register for all the events before attempting to open the channel.
                PushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                PushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                PushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                PushChannel.Open();

                // Bind this new channel for toast events.
                PushChannel.BindToShellToast();
            }
            else
            {
                // The channel was already open, so just register for all the events.
                PushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                PushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                PushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                Logs.Output.ShowOutput(String.Format("Channel Uri is {0}", PushChannel.ChannelUri.ToString()));
            }
            Logs.Output.ShowOutput("NotificationCenter constructor end");
        }

        static void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
            Logs.Output.ShowOutput(e.ChannelUri.ToString());
            try
            {
                PushChannelUri = PushChannel.ChannelUri.ToString();
            }
            catch (Exception exp)
            {
                Logs.Output.ShowOutput(exp.Message + " " + exp.InnerException);
            }
        }

        static void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Logs.Output.ShowOutput(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}", e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData));
        }

        static void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            var message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Display a dialog of all the fields in the toast.
            Logs.Output.ShowOutput(message.ToString());

            //if (_map != null)
            //_map.NotificationUpdateUi();
        }
    }
}
