using Microsoft.Phone.Maps.Toolkit;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Image pinImg { get; set; }
        public GeoCoordinate geoCoord { get; set; }
        private string pinName { get; set; }
        private string pinContent { get; set; }

        public PMMapPushpinModel(string name, string content, GeoCoordinate pos, PinType type)
        {
            pinImg = new Image();
            pinImg.Width = 100;
            pinImg.Height = 100;

            pinName = name;
            pinContent = content;
            geoCoord = pos;
            pinType = type;
        }

        public void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("Pin tapped!");
        }
    }
}
