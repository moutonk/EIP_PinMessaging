using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinMessaging.Controller
{
    class PMMapController
    {
        
    }

    static class PMMapPushpinController
    {
        public static MapLayer mapLayer {get; set;}

        public static void AddPushpinToMap(PMMapPushpinModel pin)
        {
            MapOverlay overlay = new MapOverlay();

            overlay.Content = pin.pushpin;
            overlay.GeoCoordinate = pin.pushpin.GeoCoordinate;

            if (mapLayer != null)
                mapLayer.Add(overlay);
        }

        public static void RemovePushpinFromMap(PMMapPushpinModel pin)
        {
            if (mapLayer != null)
            {

            }
        }
    }
}
