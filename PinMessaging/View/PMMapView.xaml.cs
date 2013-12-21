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

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage
    {
        readonly Geolocator _geolocator = new Geolocator();
        Geoposition _geoposition = null;
        readonly MapLayer _mapLayer = new MapLayer();
        readonly MapOverlay _userSpotLayer = new MapOverlay();
        readonly UserLocationMarker _userSpot = new UserLocationMarker();

        public PMMapView()
        {
            InitializeComponent();
            PMMapPushpinController.Initialization();

            map.Layers.Add(_mapLayer);

            _userSpotLayer.Content = _userSpot;
            _userSpot.Visibility = Visibility.Collapsed;
            _mapLayer.Add(_userSpotLayer);

            PMMapPushpinController.MapLayer = _mapLayer;

            //geolocator initialization
            _geolocator.DesiredAccuracyInMeters = 1;
            _geolocator.MovementThreshold = 3;
            _geolocator.ReportInterval = 1000;
            _geolocator.PositionChanged += geolocator_PositionChanged;
            _geolocator.StatusChanged += geolocator_StatusChanged;

            UpdateLocation(_geolocator);
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                    // Location data is available
                    Debug.WriteLine( "Location is available.");


                    if (_geoposition != null)
                    {
                        UpdateMapCenter(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);
                     }
                  

                    break;

                case PositionStatus.Initializing:
                    // This status indicates that a GPS is still acquiring a fix
                    Debug.WriteLine( "A GPS device is still initializing.");
                    break;

                case PositionStatus.NoData:
                    // No location data is currently available
                    Debug.WriteLine( "Data from location services is currently unavailable.");
                    break;

                case PositionStatus.Disabled:
                    // The app doesn't have permission to access location,
                    // either because location has been turned off.
                    Debug.WriteLine( "Your location is currently turned off. " +
                         "Change your settings through the Settings charm " +
                         " to turn it back on.");
                    break;

                case PositionStatus.NotInitialized:
                    // This status indicates that the app has not yet requested
                    // location data by calling GetGeolocationAsync() or
                    // registering an event handler for the positionChanged event.
                    Debug.WriteLine( "Location status is not initialized because " +
                        "the app has not requested location data.");
                    break;

                case PositionStatus.NotAvailable:
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateLocation(_geolocator);

                if (_geoposition != null)
                {
                    UpdateMapCenter(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);
                    //Pushpin p0 = new Pushpin();
                    //Pushpin p1 = new Pushpin();

                 
                    //t.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //p0.Content = "Test1";
                    //p1.Content = "Test2";

                    //p0.Tap += p0_Tap;





                    PMMapPushpinModel pin = new PMMapPushpinModel("test",
                                                         "mdlknskdhlr!!!",
                                                         new GeoCoordinate(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude),
                                                         PinMessaging.Model.PMMapPushpinModel.PinType.TouristInfo);
                    PMMapPushpinController.AddPushpinToMap(pin);

             
                    _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);

                    //overlay0.Content = p0;
                    //overlay0.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);
                    //overlay1.Content = p1;
                    //overlay1.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude + 1, geoposition.Coordinate.Longitude);

                    //layer0.Add(overlay0);
                    //layer0.Add(overlay1);

                    

                    // UserLocationMarkerTest.GeoCoordinate = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);


                    // p.Location = new GeoCoordinate(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                    Debug.WriteLine("New location:" + _geoposition.Coordinate.Latitude.ToString() + " " + _geoposition.Coordinate.Longitude.ToString());
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
            _geoposition =  await sender.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));

            if (_geoposition != null)
            {
                    UpdateMapCenter(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (_userSpot.Visibility == Visibility.Collapsed)
                            _userSpot.Visibility = Visibility.Visible;
                        _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);
       
                    });
                   
                Debug.WriteLine(_geoposition.Coordinate.Latitude);
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