using Microsoft.Phone.Maps.Toolkit;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinMessaging.Model
{
    class PMMapPushpinModel
    {
        public Pushpin pushpin { get; set; }

        public PMMapPushpinModel(string name, string content, GeoCoordinate pos)
        {
            pushpin = new Pushpin();
            pushpin.Name = name;
            pushpin.Content = content;
            pushpin.GeoCoordinate = pos;
        }
    }
}
