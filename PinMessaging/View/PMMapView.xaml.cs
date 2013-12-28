using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Windows.Devices.Geolocation;
using System.Diagnostics;
using System.Device.Location;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Controller;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage 
    {
        readonly MapLayer _mapLayer = new MapLayer();
        readonly MapOverlay _userSpotLayer = new MapOverlay();
        readonly UserLocationMarker _userSpot = new UserLocationMarker();
        readonly PMGeoLocation _geoLocation = null;

        public PMMapView()
        {
            InitializeComponent();

            _geoLocation = new PMGeoLocation(this);
            _userSpotLayer.Content = _userSpot;
            _userSpot.Visibility = Visibility.Collapsed;
            _mapLayer.Add(_userSpotLayer);

            map.Layers.Add(_mapLayer);

            PMData.MapLayerContainer = _mapLayer;

            UpdateLocationUI();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateMapCenter();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
              // PMMapPushpinController.RemovePushpinFromMapLayer(pin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocationUI();

                if (_geoLocation.GeopositionUser != null)
                {    
                    var pin = new PMMapPushpinModel(PMMapPushpinModel.PinsType.PublicMessage, new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude));
                    pin.CompleteInitialization("test", "mdlknskdhlr!!!");

                    PMMapPushpinController.AddPushpinToMap(pin);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UpdateLocationUI()
        {
            _geoLocation.UpdateLocation();

            if (_geoLocation.GeopositionUser != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (_userSpot.Visibility == Visibility.Collapsed)
                        _userSpot.Visibility = Visibility.Visible;
                    _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude);

                });
            }
        }

        public void UpdateMapCenter()
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (_geoLocation.GeopositionUser != null)
                {
                    map.Center = new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude);
                }
            });  
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            int retValue = Design.CustomMessageBox(new[] {"Yes", "No"}, AppResources.QuitAppTitle, AppResources.QuitApp);

            //0 is Yes, 1 is No....
            if (retValue == 0)
            {
                Application.Current.Terminate();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}