using System;
using System.Diagnostics;
using System.Globalization;
using Windows.Devices.Geolocation;
using PinMessaging.Controller;
using PinMessaging.Utils;
using PinMessaging.View;

namespace PinMessaging.Other
{
    public class PMGeoLocation
    {
        public Geoposition GeopositionUser = null;

        readonly Geolocator _geolocatorUser = new Geolocator();
        readonly PMMapView _mapView = null;
        bool _firstPositionChanged = false;

        public PMGeoLocation(PMMapView mapView)
        {
            _mapView = mapView;

            //geolocator initialization
            _geolocatorUser.DesiredAccuracyInMeters = 1;
            _geolocatorUser.MovementThreshold = 0;
            _geolocatorUser.ReportInterval = 1000;
            _geolocatorUser.PositionChanged += geolocator_PositionChanged;
            _geolocatorUser.StatusChanged += geolocator_StatusChanged;
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                    // Location data is available
                    Logs.Output.ShowOutput("Location is available.");

                    _mapView.UpdateMapCenter();
                    break;

                case PositionStatus.Initializing:
                    // This status indicates that a GPS is still acquiring a fix
                    Logs.Output.ShowOutput("A GPS device is still initializing.");
                    break;

                case PositionStatus.NoData:
                    // No location data is currently available
                    Logs.Output.ShowOutput("Data from location services is currently unavailable.");
                    break;

                case PositionStatus.Disabled:
                    // The app doesn't have permission to access location,
                    // either because location has been turned off.
                    Logs.Output.ShowOutput("Your location is currently turned off. " +
                         "Change your settings through the Settings charm " +
                         " to turn it back on.");
                    break;

                case PositionStatus.NotInitialized:
                    // This status indicates that the app has not yet requested
                    // location data by calling GetGeolocationAsync() or
                    // registering an event handler for the positionChanged event.
                    Logs.Output.ShowOutput("Location status is not initialized because " +
                        "the app has not requested location data.");
                    break;

                case PositionStatus.NotAvailable:
                    // Location is not available on this version of Windows
                    Logs.Output.ShowOutput("You do not have the required location services " +
                        "present on your system.");
                    break;

                default:
                    Logs.Output.ShowOutput("Unknown status");
                    break;
            }
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Logs.Output.ShowOutput("New pos dispo: " + args.Position.Coordinate.Latitude + " " + args.Position.Coordinate.Longitude);

            _mapView.UpdateLocationUI();

            if (_firstPositionChanged == false)
            {
                _mapView.UpdateMapCenter();
                _firstPositionChanged = true;

                var pc = new PMPinController();
                pc.GetPins(Phone.ConvertDoubleCommaToPoint(args.Position.Coordinate.Latitude.ToString()),
                           Phone.ConvertDoubleCommaToPoint(args.Position.Coordinate.Longitude.ToString()));

            }
        }

        public async void UpdateLocation()
        {
            GeopositionUser = await _geolocatorUser.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
        }
    }
}
