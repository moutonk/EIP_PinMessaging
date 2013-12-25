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
            PMMapPushpinController.Initialization();

            _geoLocation = new PMGeoLocation(this);

            map.Layers.Add(_mapLayer);

            _userSpotLayer.Content = _userSpot;
            _userSpot.Visibility = Visibility.Collapsed;
            _mapLayer.Add(_userSpotLayer);

            PMMapPushpinController.MapLayer = _mapLayer;



            UpdateLocation(_geoLocation._geolocator);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocation(_geoLocation._geolocator);

                if (_geoLocation._geoposition != null)
                {
                    UpdateMapCenter(_geoLocation._geoposition.Coordinate.Latitude, _geoLocation._geoposition.Coordinate.Longitude);
                    //Pushpin p0 = new Pushpin();
                    //Pushpin p1 = new Pushpin();

                 
                    //t.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //p0.Content = "Test1";
                    //p1.Content = "Test2";

                    //p0.Tap += p0_Tap;





                    PMMapPushpinModel pin = new PMMapPushpinModel("test",
                                                         "mdlknskdhlr!!!",
                                                         new GeoCoordinate(_geoLocation._geoposition.Coordinate.Latitude, _geoLocation._geoposition.Coordinate.Longitude),
                                                         PinMessaging.Model.PMMapPushpinModel.PinType.TouristInfo);
                    PMMapPushpinController.AddPushpinToMap(pin);


                    _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoLocation._geoposition.Coordinate.Latitude, _geoLocation._geoposition.Coordinate.Longitude);

                    //overlay0.Content = p0;
                    //overlay0.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                    //overlay1.Content = p1;
                    //overlay1.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude + 1, geoposition.Coordinate.Longitude);

                    //layer0.Add(overlay0);
                    //layer0.Add(overlay1);

                    

                    // UserLocationMarkerTest.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);


                    // p.Location = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    Debug.WriteLine("New location:" + _geoLocation._geoposition.Coordinate.Latitude.ToString() + " " + _geoLocation._geoposition.Coordinate.Longitude.ToString());
                }
    
            

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async void UpdateLocation(Geolocator sender)
        {
            _geoLocation._geoposition = await sender.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));

            if (_geoLocation._geoposition != null)
            {
                    UpdateMapCenter(_geoLocation._geoposition.Coordinate.Latitude, _geoLocation._geoposition.Coordinate.Longitude);
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (_userSpot.Visibility == Visibility.Collapsed)
                            _userSpot.Visibility = Visibility.Visible;
                        _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoLocation._geoposition.Coordinate.Latitude, _geoLocation._geoposition.Coordinate.Longitude);
       
                    });

                    Debug.WriteLine(_geoLocation._geoposition.Coordinate.Latitude);
            }
        }

        public void UpdateMapCenter(double latitude, double longitude)
        {
            Dispatcher.BeginInvoke(() =>
            {
                map.Center = new GeoCoordinate(latitude, longitude);
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