using System;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.Windows.Controls;
using PinMessaging.Utils;

namespace PinMessaging.Model
{
    public class PMMapPushpinModel
    {
        public enum PinsType
        {
            PublicMessage,
            PrivateMessage,
            Event,
            CourseLastStep,
            Eye,
            PointOfInterest
        }

        [DefaultValue(PinsType.PublicMessage)] public PinsType PinType { get; set; }
        [DefaultValue(null)] public Image PinImg { get; private set; }
        [DefaultValue(null)] public GeoCoordinate GeoCoord { get; private set; }
        [DefaultValue("")] private string PinName { get; set; }
        [DefaultValue("")] private string PinContent { get; set; }

        public PMMapPushpinModel(PinsType type, GeoCoordinate pos)
        {
            PinImg = new Image();
            GeoCoord = pos;
            PinType = type;
        }

        public void CompleteInitialization(string name, string content)
        {
            PinName = name;
            PinContent = content;
        }

        public void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Logs.Output.ShowOutput("Pin tapped!");
        }
    }
}
