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
using PinMessaging.Model;
using System.Windows.Shapes;
using System.Windows.Media;
using PinMessaging.Controller;

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage
    {
        Geolocator geolocator = new Geolocator();
        Geoposition geoposition = null;
        MapLayer mapLayer = new MapLayer();
        MapOverlay userSpotLayer = new MapOverlay();
        UserLocationMarker userSpot = new UserLocationMarker();

        public PMMapView()
        {
            InitializeComponent();
            PMMapPushpinController.Initialization();

            map.Layers.Add(mapLayer);

            userSpotLayer.Content = userSpot;

            userSpot.Visibility = Visibility.Collapsed;

            mapLayer.Add(userSpotLayer);

            PMMapPushpinController.mapLayer = mapLayer;

            //geolocator initialization
            geolocator.DesiredAccuracyInMeters = 1;
            geolocator.MovementThreshold = 3;
            geolocator.ReportInterval = 1000;
            geolocator.PositionChanged += geolocator_PositionChanged;
            geolocator.StatusChanged += geolocator_StatusChanged;

         
            UpdateLocation(geolocator);
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case Windows.Devices.Geolocation.PositionStatus.Ready:
                    // Location data is available
                    Debug.WriteLine( "Location is available.");


                    if (geoposition != null)
                    {
                        UpdateMapCenter(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                     }
                  

                    break;

                case Windows.Devices.Geolocation.PositionStatus.Initializing:
                    // This status indicates that a GPS is still acquiring a fix
                    Debug.WriteLine( "A GPS device is still initializing.");
                    break;

                case Windows.Devices.Geolocation.PositionStatus.NoData:
                    // No location data is currently available
                    Debug.WriteLine( "Data from location services is currently unavailable.");
                    break;

                case Windows.Devices.Geolocation.PositionStatus.Disabled:
                    // The app doesn't have permission to access location,
                    // either because location has been turned off.
                    Debug.WriteLine( "Your location is currently turned off. " +
                         "Change your settings through the Settings charm " +
                         " to turn it back on.");
                    break;

                case Windows.Devices.Geolocation.PositionStatus.NotInitialized:
                    // This status indicates that the app has not yet requested
                    // location data by calling GetGeolocationAsync() or
                    // registering an event handler for the positionChanged event.
                    Debug.WriteLine( "Location status is not initialized because " +
                        "the app has not requested location data.");
                    break;

                case Windows.Devices.Geolocation.PositionStatus.NotAvailable:
                    // Location is not available on this version of Windows
                    Debug.WriteLine( "You do not have the required location services " +
                        "present on your system.");
                    break;

                default:
                    Debug.WriteLine( "Unknown status");
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocation(geolocator);

                if (geoposition != null)
                {
                    UpdateMapCenter(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                    //Pushpin p0 = new Pushpin();
                    //Pushpin p1 = new Pushpin();

                 
                    //t.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //p0.Content = "Test1";
                    //p1.Content = "Test2";

                    //p0.Tap += p0_Tap;





                    PMMapPushpinModel pin = new PMMapPushpinModel("test",
                                                         "mdlknskdhlr!!!",
                                                         new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude),
                                                         PinMessaging.Model.PMMapPushpinModel.PinType.TouristInfo);
                    PMMapPushpinController.AddPushpinToMap(pin);

             
                    userSpotLayer.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //overlay0.Content = p0;
                    //overlay0.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                    //overlay1.Content = p1;
                    //overlay1.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude + 1, geoposition.Coordinate.Longitude);

                    //layer0.Add(overlay0);
                    //layer0.Add(overlay1);

                    

                    // UserLocationMarkerTest.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);


                    // p.Location = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    Debug.WriteLine("New location:" + geoposition.Coordinate.Latitude.ToString() + " " + geoposition.Coordinate.Longitude.ToString());
                }
    
            

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

        private async  void UpdateLocation(Geolocator sender)
        {
            geoposition =  await sender.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));

            if (geoposition != null)
            {
                    UpdateMapCenter(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (userSpot.Visibility == Visibility.Collapsed)
                            userSpot.Visibility = Visibility.Visible;
                        userSpotLayer.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
       
                    });
                   
                Debug.WriteLine(geoposition.Coordinate.Latitude);
            }
        }

        private void UpdateMapCenter(double latitude, double longitude)
        {
            Dispatcher.BeginInvoke(() =>
            {
                map.Center = new GeoCoordinate(latitude, longitude);
            });  
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            
            Debug.WriteLine("new pos dispo:" + args.Position.Coordinate.Latitude + " " + args.Position.Coordinate.Longitude);
         
            UpdateLocation(sender);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}