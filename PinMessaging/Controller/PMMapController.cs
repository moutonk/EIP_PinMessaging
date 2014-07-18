using System.Linq;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using PinMessaging.Utils.WebService;
using PinMessaging.View;

namespace PinMessaging.Controller
{
    public class PMMapController : PMWebServiceEndDetector
    {
        public PMMapController(RequestType currentRequestType)
        {
            CurrentRequestType = currentRequestType;
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                StopTimer();

                Logs.Output.ShowOutput("should not be here");
            }
        }
    }

    public static class PMMapContactController
    {
        private static PMMapView _mapView = null;

        public static void Init(PMMapView mapview)
        {
            _mapView = mapview;
        }

        public static void AddNewFavoris(PMUserModel user)
        {
            _mapView.AddContact(user);
        }

        public static void RemoveFavoris(PMUserModel user)
        {
            _mapView.RemoveContact(user);
        }

        public static bool IsFavoriteUnique(PMUserModel user)
        {
            //does the list contains an element with the same id?
            var val = PMData.UserList.Any(listPin => listPin.Id == user.Id);

            Logs.Output.ShowOutput(val == true ? "User is not unique" : "User is unique");

            return !val;
        }
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

            //Logs.Output.ShowOutput(val == false ? "Pin is not unique" : "Pin is unique");

            return val;
        }

        public static void AddPinToMap(PMPinModel pin)
        {
            var overlay = new MapOverlay
            {
                PositionOrigin = new Point(0.3, 0.8),
                Content = pin.PinImg,
                GeoCoordinate = pin.GeoCoord
            };

            //center the mapoverlay, will change later

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
            var pinLongitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Longitude);
            var pinLatitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Latitude);

            if (_mapView != null && _mapView._geoLocation != null)
            {
                var userLongitude = _mapView._geoLocation.GeopositionUser.Coordinate.Longitude;
                var userLatitude = _mapView._geoLocation.GeopositionUser.Coordinate.Latitude;

                Logs.Output.ShowOutput("Distance:" + (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2))).ToString());
            }

            return true;
        }

        public static void DropPrivatePin(PMUserModel user)
        {
            _mapView.DropPrivatePin(user);
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
