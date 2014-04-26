using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using PinMessaging.View;

namespace PinMessaging.Controller
{
    public static class PMMapController
    {
        
    }

    public static class PMMapPinController
    {
        private static PMMapView _mapView = null;

        public static void Init(PMMapView mapview)
        {
            _mapView = mapview;
        }

        public static bool IsPinUnique(PMPinModel pin)
        {
            bool val = !PMData.PinsList.Any(listPin => listPin.Id == pin.Id);

            Logs.Output.ShowOutput(val == false ? "Pin is not unique" : "Pin is unique");

            return val;
        }

        public static void AddPinToMap(PMPinModel pin)
        {
            var overlay = new MapOverlay {PositionOrigin = new Point(0.3, 1)};

            //center the mapoverlay, will change later

            overlay.Content = pin.PinImg;
            overlay.GeoCoordinate = pin.GeoCoord;

            if (PMData.MapLayerContainer != null)
                PMData.MapLayerContainer.Add(overlay);
        }

        public static void RemovePinFromMap(PMPinModel pin)
        {
            if (PMData.MapLayerContainer != null)
            {
            //    var res = from mapLayerElem in MapLayerContainer
            //              where pin == mapLayerElem.Content
            //              select mapLayerElem;
            }
        }

        private static bool IsAroundMe(PMPinModel pin)
        {
            double pinLongitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Location["longitude"]);
            double pinLatitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Location["latitude"]);

            if (_mapView != null && _mapView._geoLocation != null)
            {
                double userLongitude = _mapView._geoLocation.GeopositionUser.Coordinate.Longitude;
                double userLatitude = _mapView._geoLocation.GeopositionUser.Coordinate.Latitude;

                Logs.Output.ShowOutput("Distance:" + (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2))).ToString());
            }

            return true;
        }

        public static void OnPinTapped(PMPinModel pin)
        {
            if (IsAroundMe(pin) == true)
            {
                pin.ShowPinContent();
                _mapView.PinTapped(pin);
            }
        }
    }
}
