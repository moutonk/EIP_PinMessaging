using System.Device.Location;
using System.Linq;
using System.Windows.Controls;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using System;
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

        public static void CloseDownMenu()
        {
            _mapView.CloseMenuDownButton_Click(null, null);
        }

        public static void MapCenterOn(GeoCoordinate coord)
        {
            _mapView.MapCenterOn(coord);
        }
        
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
            try
            {
                //does the list contains an element with the same id?
                var val = PMData.UserList.Any(listPin => listPin.Id == user.Id);

                Logs.Output.ShowOutput(val == true ? "User is not unique" : "User is unique");

                return !val;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("IsFavoriteUnique: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
                return false;
            }
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
            try
            {
                return PMData.PinsList.All(listPin => listPin.Id != pin.Id);
                //Logs.Output.ShowOutput(val == false ? "Pin is not unique" : "Pin is unique");

            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("IsPinUnique" + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
                return true;
            }
        }

        public static void AddPinToMap(PMPinModel pin)
        {
            pin.PinImg.Tag = pin;

            var overlay = new MapOverlay
            {
                PositionOrigin = new Point(0.3, 0.8),
                Content = pin.PinImg,
                GeoCoordinate = pin.GeoCoord
            };

            //hidden
            if (PMData.HiddenTypesList.Contains(Utils.Utils.PinPrivateToPinType(pin)))
            {
                PMData.FilterMapOverlayExcessItemList.Add(overlay);
            }
            else // normal
            {
                if (PMData.MapLayerContainer != null)
                    PMData.MapLayerContainer.Add(overlay);
            }
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

        public static void HideOrNotPinType(PMPinModel.PinsType type, bool hidden)
        {
            if (PMData.MapLayerContainer == null)
            {
                Logs.Error.ShowError("HideOrNotPinType: MapLayerContainer is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var uselessVar = new Image();

            if (hidden == true)
            {
                for (var pos = 0; pos < PMData.MapLayerContainer.Count; pos++)
                {
                    //if the object is not a MapOverlay we loop again
                    if (PMData.MapLayerContainer[pos].Content.IsTypeOf(uselessVar) == false)
                        continue;

                    var img = PMData.MapLayerContainer[pos].Content as Image;

                    if (img == null || img.Tag == null)
                    {
                        Logs.Error.ShowError("HideOrNotPinType: " + (img == null ? "img" : "Tag") + " is null", Logs.Error.ErrorsPriority.NotCritical);
                        continue;
                    }

                    var pin = img.Tag as PMPinModel;

                    if (pin == null)
                    {
                        Logs.Error.ShowError("HideOrNotPinType: pin is null", Logs.Error.ErrorsPriority.NotCritical);
                        continue;
                    }

                    if (type != Utils.Utils.PinPrivateToPinType(pin))
                        continue;
                    
                    var tmpItem = PMData.MapLayerContainer[pos];
                    PMData.MapLayerContainer.RemoveAt(pos);
                    PMData.FilterMapOverlayExcessItemList.Add(tmpItem);
                    pos--;
                }
            }
            else
            {
                for (var pos = 0; pos < PMData.FilterMapOverlayExcessItemList.Count; pos++)
                {
                    //if the object is not a MapOverlay we loop again
                    if (PMData.FilterMapOverlayExcessItemList[pos].Content.IsTypeOf(uselessVar) == false)
                        continue;

                    var img = PMData.FilterMapOverlayExcessItemList[pos].Content as Image;

                    if (img == null || img.Tag == null)
                    {
                        Logs.Error.ShowError("HideOrNotPinType: " + (img == null ? "img" : "Tag") + " is null", Logs.Error.ErrorsPriority.NotCritical);
                        continue;
                    }

                    var pin = img.Tag as PMPinModel;

                    if (pin == null)
                    {
                        Logs.Error.ShowError("HideOrNotPinType: pin is null", Logs.Error.ErrorsPriority.NotCritical);
                        continue;
                    }

                    if (type != Utils.Utils.PinPrivateToPinType(pin))
                        continue;

                    var tmpItem = PMData.FilterMapOverlayExcessItemList[pos];
                    PMData.FilterMapOverlayExcessItemList.RemoveAt(pos);
                    PMData.MapLayerContainer.Add(tmpItem);
                    pos--;
                }
            }
        }

        private static bool IsAroundMe(PMPinModel pin)
        {
            if (pin == null)
            {
                Logs.Error.ShowError("IsAroundMe: pin is null", Logs.Error.ErrorsPriority.NotCritical);
                return false;
            }

            var pinLongitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Longitude);
            var pinLatitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Latitude);

            if (_mapView == null || _mapView._geoLocation == null || _mapView._geoLocation.GeopositionUser == null)
            {
                Logs.Error.ShowError("IsAroundMe: _geoLocation or GeopositionUser is null", Logs.Error.ErrorsPriority.NotCritical);
                return false;
            }

            var userLongitude = _mapView._geoLocation.GeopositionUser.Coordinate.Point.Position.Longitude;
            var userLatitude = _mapView._geoLocation.GeopositionUser.Coordinate.Point.Position.Latitude;

            Logs.Output.ShowOutput("Distance:" + (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2))));

            //if (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2)) > 0.004d)
            //   return false;

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
                if (pin != null)
                    pin.ShowPinContent();
                _mapView.PinTapped(pin);
            }
            else
            {
                Logs.Output.ShowOutput("Too far!");
            }
        }

        public static string GetPinTitle(string pinId)
        {
            try
            {
                return PMData.PinsList.Find(pin => pin.Id.Equals(pinId) == true).Title;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("GetPinTitle: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
            return "unknown pin";
        }
    }

    public static class PMMapNotifController
    {
        private static PMMapView _mapView = null;

        public static void Init(PMMapView mapview)
        {
            _mapView = mapview;
        }

        public static void AddNotifToUi(PMNotificationModel notif)
        {
            _mapView.NotificationAddItem(notif);
        }
    }
}
