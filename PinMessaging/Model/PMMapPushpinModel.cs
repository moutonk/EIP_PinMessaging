using System.Device.Location;
using System.Diagnostics;
using System.Windows.Controls;

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

        public PinsType PinType { get; set; }
        public Image PinImg { get; set; }
        public GeoCoordinate GeoCoord { get; set; }
        private string PinName { get; set; }
        private string PinContent { get; set; }

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
            Debug.WriteLine("Pin tapped!");
        }
    }
}
