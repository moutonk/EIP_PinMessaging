using System;
using System.Text;
using Microsoft.Phone.Notification;
using PinMessaging.Utils;
using PinMessaging.View;

namespace PinMessaging.Other
{
    class NotificationCenter
    {
        enum NotificationType
        {
            NewComment = 0,
            PinModified,
            NewGrade,
            NewFavorite
        }

        /// Holds the push channel that is created or found.
        private HttpNotificationChannel _pushChannel;
        private const string ChannelName = "ToastChannel";
        private PMMapView _map = null;

        public void Init(PMMapView map)
        {
            _map = map;

            // Try to find the push channel.
            _pushChannel = HttpNotificationChannel.Find(ChannelName);

            // If the channel was not found, then create a new connection to the push service.
            if (_pushChannel == null)
            {
                _pushChannel = new HttpNotificationChannel(ChannelName);

                // Register for all the events before attempting to open the channel.
                _pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                _pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                _pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);
                
                _pushChannel.Open();

                // Bind this new channel for toast events.
                _pushChannel.BindToShellToast();

            }
            else
            {
                // The channel was already open, so just register for all the events.
                _pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                _pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                _pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                // Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                Logs.Output.ShowOutput(String.Format("Channel Uri is {0}", _pushChannel.ChannelUri.ToString()));

            }
        }

        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
            Logs.Output.ShowOutput(e.ChannelUri.ToString());
        }

        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Logs.Output.ShowOutput(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}", e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData));
        }

        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
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
