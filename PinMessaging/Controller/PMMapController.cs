﻿using System.Device.Location;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Devices.Geolocation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
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
            pin.PinImg.Tag = pin;

            var overlay = new MapOverlay
            {
                PositionOrigin = new Point(0.3, 0.8),
                Content = pin.PinImg,
                GeoCoordinate = pin.GeoCoord
            };

            //hidden
            if (PMData.HiddenTypesList.Contains(Utils.Utils.PinToPinType(pin)))
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
            if (PMData.MapLayerContainer != null)
            {
                var uselessVar = new Image();

                if (hidden == true)
                {
                    for (var pos = 0; pos < PMData.MapLayerContainer.Count; pos++)
                    {
                        //if the object is not a MapOverlay we loop again
                        if (PMData.MapLayerContainer[pos].Content.IsTypeOf(uselessVar) == false || (PMData.MapLayerContainer[pos].Content as Image).Tag == null)
                            continue;

                        if (type == Utils.Utils.PinToPinType((PMData.MapLayerContainer[pos].Content as Image).Tag as PMPinModel))
                        {
                            var tmpItem = PMData.MapLayerContainer[pos];
                            PMData.MapLayerContainer.RemoveAt(pos);
                            PMData.FilterMapOverlayExcessItemList.Add(tmpItem);
                            pos--;
                        }
                    }
                }
                else
                {
                    for (var pos = 0; pos < PMData.FilterMapOverlayExcessItemList.Count; pos++)
                    {
                        //if the object is not a MapOverlay we loop again
                        if (PMData.FilterMapOverlayExcessItemList[pos].Content.IsTypeOf(uselessVar) == false || (PMData.FilterMapOverlayExcessItemList[pos].Content as Image).Tag == null)
                            continue;

                        if (type == Utils.Utils.PinToPinType((PMData.FilterMapOverlayExcessItemList[pos].Content as Image).Tag as PMPinModel))
                        {
                            var tmpItem = PMData.FilterMapOverlayExcessItemList[pos];
                            PMData.FilterMapOverlayExcessItemList.RemoveAt(pos);
                            PMData.MapLayerContainer.Add(tmpItem);
                            pos--;
                        }
                    }
                }
            }
        }

        private static bool IsAroundMe(PMPinModel pin)
        {
            var pinLongitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Longitude);
            var pinLatitude = Utils.Utils.ConvertDoubleCommaToPoint(pin.Latitude);

            if (_mapView != null && _mapView._geoLocation != null && _mapView._geoLocation.GeopositionUser != null)
            {
                var userLongitude = _mapView._geoLocation.GeopositionUser.Coordinate.Longitude;
                var userLatitude = _mapView._geoLocation.GeopositionUser.Coordinate.Latitude;

                Logs.Output.ShowOutput("Distance:" + (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2))));

                //if (Math.Sqrt(Math.Pow(pinLongitude - userLongitude, 2) + Math.Pow(pinLatitude - userLatitude, 2)) > 0.004d)
                 //   return false;
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
            else
            {
                Logs.Output.ShowOutput("Too far!");
            }
        }
    }
}
