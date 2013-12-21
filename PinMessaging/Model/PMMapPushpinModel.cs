using System.Device.Location;
using System.Diagnostics;
using System.Windows.Controls;

namespace PinMessaging.Model
{
    public class PMMapPushpinModel
    {
        public enum PinType
        {
            PublicMessage,
            PrivateMessage,
            TouristInfo,
            Event,
            TreasureHunt
        }

        public PinType pinType { get; set; }
        public Image PinImg { get; set; }
        public GeoCoordinate GeoCoord { get; set; }
        private string PinName { get; set; }
        private string PinContent { get; set; }

        public PMMapPushpinModel(string name, string content, GeoCoordinate pos, PinType type)
        {
            PinImg = new Image {Width = 100, Height = 100};
            PinName = name;
            PinContent = content;
            GeoCoord = pos;
            pinType = type;
        }

        public void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("Pin tapped!");
        }
    }
}
