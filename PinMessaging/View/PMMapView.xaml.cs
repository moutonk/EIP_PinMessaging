using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Diagnostics;
using System.Device.Location;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage
    {
        Geolocator geolocator = new Geolocator();
        Geoposition geoposition;

        public PMMapView()
        {
            InitializeComponent();

            //geolocator initialization
            geolocator.DesiredAccuracyInMeters = 1;
            geolocator.MovementThreshold = 1;
            geolocator.ReportInterval = 1000;
            geolocator.PositionChanged += geolocator_PositionChanged;

            //map initialization
            map.ZoomLevel = 16;
            map.LandmarksEnabled = true;
            map.PedestrianFeaturesEnabled = true;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocation(geolocator);
                UpdateMapCenter(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                Pushpin p0 = new Pushpin();
                Pushpin p1 = new Pushpin();

                p0.Content = "Test1";
                p1.Content = "Test2";

                p0.Tap += p0_Tap;

                MapLayer layer0 = new MapLayer();
                
                MapOverlay overlay0 = new MapOverlay();
                MapOverlay overlay1 = new MapOverlay();

                overlay0.Content = p0;
                overlay0.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                overlay1.Content = p1;
                overlay1.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude +1, geoposition.Coordinate.Longitude);

                layer0.Add(overlay0);
                layer0.Add(overlay1);

                map.Layers.Add(layer0);
               
               // UserLocationMarkerTest.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);


               // p.Location = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                Debug.WriteLine("New location:" + geoposition.Coordinate.Latitude.ToString() + " " + geoposition.Coordinate.Longitude.ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void p0_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("tap");
        }

        private async void UpdateLocation(Geolocator sender)
        {
            geoposition = await sender.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
        }
        
        private void UpdateMapCenter(double latitude, double longitude)
        {
            map.Center = new GeoCoordinate(latitude, longitude);
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            UpdateLocation(sender);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}