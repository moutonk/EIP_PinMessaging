using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
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

            Map.Layers.Add(_mapLayer);

            PMData.MapLayerContainer = _mapLayer;

            UpdateLocationUI();
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

                    PMMapPinController.AddPushpinToMap(pin);
                }
            }
            catch (Exception ex)
            {
                Logs.Output.ShowOutput(ex.Message);
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
                    Map.Center = new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude);
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Map.ZoomLevel <= 19D)
                Map.ZoomLevel += 1D;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Map.ZoomLevel >= 2D)
            Map.ZoomLevel -= 1D;
        }

        private void Center_Click(object sender, RoutedEventArgs e)
        {
            UpdateMapCenter();
        }

        ////////////////MANAGE SWIPE LEFT RIGHT/////////////////////////

        private const int LeftPage = -420;
        private const int MiddlePage = 0;
        private const int RightPage = -840;
        private const int Overlap = 100;

        private double _initialPosition;
        private bool _viewMoved = false;

        private void OpenClose_Left(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);

            MoveViewWindow(left > -Overlap ? LeftPage : MiddlePage);
        }

        private void OpenClose_Right(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);

            MoveViewWindow(left > LeftPage - Overlap ? RightPage : LeftPage);
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;

            moveAnimation.SkipToFill();
            ((DoubleAnimation)(moveAnimation).Children[0]).To = left;
            moveAnimation.Begin();
        }

        private void canvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.DeltaManipulation.Translation.X != 0)
                Canvas.SetLeft(LayoutRoot, Math.Min(Math.Max(RightPage, Canvas.GetLeft(LayoutRoot) + e.DeltaManipulation.Translation.X), 0));
        }

        private void canvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _viewMoved = false;
            _initialPosition = Canvas.GetLeft(LayoutRoot);
        }

        private void canvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);

            if (_viewMoved)
                return;

            if (Math.Abs(_initialPosition - left) < Overlap)
            {
                //bouncing back
                MoveViewWindow(_initialPosition);
                return;
            }

            //change of state
            if (_initialPosition - left > MiddlePage)
            {
                MoveViewWindow(_initialPosition > LeftPage ? LeftPage : RightPage);
            }
            else
            {
                //slide to the right
                MoveViewWindow(_initialPosition < LeftPage ? LeftPage : MiddlePage);
            }
        }

        private void ButtonMap_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonProfil_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonPins_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonFilters_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonReward_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}