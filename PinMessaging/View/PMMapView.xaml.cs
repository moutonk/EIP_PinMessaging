using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Phone.Controls;
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
        private enum CurrentMapPageView { MapView, LeftMenuView, RightMenuView, UnderMenuView}

        private static CurrentMapPageView _currentView = CurrentMapPageView.MapView;
        private static bool _isUnderMenuOpen = false;
     
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

            LoadRessources();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
              // PMMapPushpinController.RemovePushpinFromMapLayer(pin);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pc = new PMPinController();

     //       pc.GetPins(Phone.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Latitude.ToString()),
       //                    Phone.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Longitude.ToString()));
            pc.CreatePin(_geoLocation.GeopositionUser, new[] {"Super name",
                                                                "Ceci est un pin de test", 
                                                                ((int)PMPinModel.PinsType.Event).ToString()});

//                    string json = @"[
//                                   {                                
//                                        ""id"":""1"",
//                                        ""type"":""Event"",
//                                        ""description"":""\\home\\"",
//                                        ""url"":""null"",
//                                        ""lang"":""null"",
//                                        ""location"":
//                                        {
//                                            ""id"":""1"",
//                                            ""latitude"":""47.669444"",
//                                            ""longitude"":""-122.123889"",
//                                            ""name"":""\\maison\\""
//                                        },
//                                        ""creationTime"":""1392003691000""
//                                    },
//                                    {                                
//                                        ""id"":""2"",
//                                        ""type"":""Lol"",
//                                        ""description"":""\\home\\"",
//                                        ""url"":""null"",
//                                        ""lang"":""null"",
//                                        ""location"":
//                                        {
//                                            ""id"":""1"",
//                                            ""latitude"":""48.669450"",
//                                            ""longitude"":""-122.123850"",
//                                            ""name"":""\\maison\\""
//                                        },
//                                        ""creationTime"":""1392003691000""
//                                    }
//                                ]";
//                PMDataConverter.ParseGetPins(json);
                    //var pin = new PMMapPushpinModel(PMMapPushpinModel.PinsType.PublicMessage, new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude));
                    //pin.CompleteInitialization("test", "mdlknskdhlr!!!");

                    //PMMapPinController.AddPushpinToMap(pin);
        
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

        private void MenuDown_OnClick(object sender, RoutedEventArgs e)
        {
            _currentView = CurrentMapPageView.UnderMenuView;
            _isUnderMenuOpen = true;
            _enableSwipe = false;
            
            MainGridMap.RowDefinitions[2].Height = new GridLength(0);
            MoveAnimationUp.Begin();
        }

        private void Map_OnTouch(object sender, RoutedEventArgs e)
        {
            if (_isUnderMenuOpen == true)
            {
                MainGridMap.RowDefinitions[2].Height = new GridLength(90);
           
                _currentView = CurrentMapPageView.MapView;
                _isUnderMenuOpen = false;
                _enableSwipe = true;
                MoveAnimationDown.Begin();
            }

            if (_currentView == CurrentMapPageView.LeftMenuView)
            {
                OpenClose_Left(sender, e);
            }
            else if (_currentView == CurrentMapPageView.RightMenuView)
            {
                OpenClose_Right(sender, e);
            }
        }

        ////////////////MANAGE SWIPE LEFT RIGHT/////////////////////////

        private bool _enableSwipe = true;
        private double _leftPage;
        private const int MiddlePage = 0;
        private double _rightPage;
        private const int Overlap = 100;

        private double _initialPosition;
        private bool _viewMoved = false;

        private void LoadRessources()
        {
            try
            {
                _leftPage = -(double)Application.Current.Resources["AdditionalMapMenuWidth"];
                _rightPage = -2 * (double)Application.Current.Resources["AdditionalMapMenuWidth"];
            }
            catch (Exception)
            {
                Logs.Error.ShowError("Could not load AdditionalMapMenuWidth from App.xaml", Logs.Error.ErrorsPriority.NotCritical);
                _leftPage = -420;
                _rightPage = -840;
            }
        }

        private void OpenClose_Left(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);

            if (left > -Overlap)
            {
                _currentView = CurrentMapPageView.MapView;
                MoveViewWindow(_leftPage);
            }
            else
            {
                _currentView = CurrentMapPageView.LeftMenuView;
                MoveViewWindow(MiddlePage);
            }  
        }

        private void OpenClose_Right(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);

            if (left > _leftPage - Overlap)
            {
                _currentView = CurrentMapPageView.RightMenuView;
                MoveViewWindow(_rightPage);
            }
            else
            {
                _currentView = CurrentMapPageView.MapView;
                MoveViewWindow(_leftPage);
            }      
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
            if (_enableSwipe == true)     
                if (e.DeltaManipulation.Translation.X != 0)
                    Canvas.SetLeft(LayoutRoot, Math.Min(Math.Max(_rightPage, Canvas.GetLeft(LayoutRoot) + e.DeltaManipulation.Translation.X), 0));
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
                MoveViewWindow(_initialPosition > _leftPage ? _leftPage : _rightPage);
            }
            else
            {
                //slide to the right
                MoveViewWindow(_initialPosition < _leftPage ? _leftPage : MiddlePage);
            }
        }

        ////////////////////////////////////////////////    Left Menu    /////////////////////////////////////////////

        private void ButtonMap_OnClick(object sender, RoutedEventArgs e)
        {
            OpenClose_Left(sender, e);
        }

        private void ButtonProfil_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonPins_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonFilters_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonReward_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                 NavigationService.Navigate(Paths.FirstLaunch);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }
    }
}