using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using PinMessaging.View;

namespace PinMessaging.Other
{
    class PMGeoLocation
    {
        public readonly Geolocator _geolocator = new Geolocator();
        public Geoposition _geoposition = null;
        private readonly PMMapView _mapView = null;

        public PMGeoLocation(PMMapView mapView)
        {
            _mapView = mapView;

            //geolocator initialization
            _geolocator.DesiredAccuracyInMeters = 1;
            _geolocator.MovementThreshold = 3;
            _geolocator.ReportInterval = 1000;
            _geolocator.PositionChanged += geolocator_PositionChanged;
            _geolocator.StatusChanged += geolocator_StatusChanged;
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                    // Location data is available
                    Debug.WriteLine("Location is available.");

                    if (_geoposition != null)
                    {
                        _mapView.UpdateMapCenter(_geoposition.Coordinate.Latitude, _geoposition.Coordinate.Longitude);
                    }
                    break;

                case PositionStatus.Initializing:
                    // This status indicates that a GPS is still acquiring a fix
                    Debug.WriteLine("A GPS device is still initializing.");
                    break;

                case PositionStatus.NoData:
                    // No location data is currently available
                    Debug.WriteLine("Data from location services is currently unavailable.");
                    break;

                case PositionStatus.Disabled:
                    // The app doesn't have permission to access location,
                    // either because location has been turned off.
                    Debug.WriteLine("Your location is currently turned off. " +
                         "Change your settings through the Settings charm " +
                         " to turn it back on.");
                    break;

                case PositionStatus.NotInitialized:
                    // This status indicates that the app has not yet requested
                    // location data by calling GetGeolocationAsync() or
                    // registering an event handler for the positionChanged event.
                    Debug.WriteLine("Location status is not initialized because " +
                        "the app has not requested location data.");
                    break;

                case PositionStatus.NotAvailable:
                    // Location is not available on this version of Windows
                    Debug.WriteLine("You do not have the required location services " +
                        "present on your system.");
                    break;

                default:
                    Debug.WriteLine("Unknown status");
                    break;
            }
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            Debug.WriteLine("new pos dispo:" + args.Position.Coordinate.Latitude + " " + args.Position.Coordinate.Longitude);

            _mapView.UpdateLocation(sender);
        }
    }
}
