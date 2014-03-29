using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Controller;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage, INotifyPropertyChanged
    {
        private enum CurrentMapPageView { MapView, LeftMenuView, RightMenuView, UnderMenuView}

        private static CurrentMapPageView _currentView = CurrentMapPageView.MapView;
        private static bool _isUnderMenuOpen = false;
     
        readonly MapLayer _mapLayer = new MapLayer();
        readonly MapOverlay _userSpotLayer = new MapOverlay();
        readonly UserLocationMarker _userSpot = new UserLocationMarker();
        public PMGeoLocation _geoLocation = null;

        public PMMapView()
        {
            InitializeComponent();

            //central page
            ImgTarget.ImageSource = new BitmapImage(Paths.TargetButton);
            ImgMenuButton.ImageSource = new BitmapImage(Paths.MenuButton);
            ImgNotificationButton.ImageSource = new BitmapImage(Paths.NotificationsButton);
            ImgContactsButton.ImageSource = new BitmapImage(Paths.ContactsButton);
            ImgPinsButton.ImageSource = new BitmapImage(Paths.PinsButton);

            //left menu
            ImgMap.Source = new BitmapImage(Paths.LeftMenuMap);
            ImgFilters.Source = new BitmapImage(Paths.LeftMenuFilters);
            ImgProfil.Source = new BitmapImage(Paths.LeftMenuProfil);
            ImgPins.Source = new BitmapImage(Paths.LeftMenuPins);
            ImgSettings.Source = new BitmapImage(Paths.LeftMenuSettings);
            ImgAbout.Source = new BitmapImage(Paths.LeftMenuAbout);
            ImgReward.Source = new BitmapImage(Paths.LeftMenuRewards);
            ImgLogout.Source = new BitmapImage(Paths.LeftMenuLogout);
      
            bool? accessLoc =  RememberConnection.GetAccessLocation();
            Logs.Output.ShowOutput(accessLoc.ToString());

            if (accessLoc == true)
            {
                Logs.Output.ShowOutput("accesloc true");
                LaunchLocalization();      
            }

            PMMapPinController.Init(this);
            LoadRessources();

            listPickerPins = new List<ElementsListPicker> 
            {
                new ElementsListPicker { Name = AppResources.PinPublicMessage, ImgPath = Paths.PinPublicMessage},
                new ElementsListPicker { Name = AppResources.PinPrivateMessage, ImgPath = Paths.PinPrivateMessage},
                new ElementsListPicker { Name = AppResources.PinEvent, ImgPath = Paths.PinEvent},
                new ElementsListPicker { Name = AppResources.PinPointOfView, ImgPath = Paths.PinEye},
                new ElementsListPicker { Name = AppResources.PinPointOfInterest, ImgPath = Paths.PinPointOfInterest},
             };
        }

        private void LaunchLocalization()
        {
            _geoLocation = new PMGeoLocation(this);
            _userSpotLayer.Content = _userSpot;
            _userSpot.Visibility = Visibility.Collapsed;

            try
            {
                _mapLayer.Add(_userSpotLayer);
                Map.Layers.Add(_mapLayer);
                
            }
            catch (Exception)
            {
            }


            PMData.MapLayerContainer = _mapLayer;

            UpdateLocationUI();    
        }

        public void ProgressBarActive(bool st)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (st == false)
                {
                    ProgressBarMap.IsIndeterminate = false;
                    ProgressBarMap.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ProgressBarMap.IsIndeterminate = true;
                    ProgressBarMap.Visibility = Visibility.Visible;
                }
            });
        
       }

        private int AccessLocationMsgBox()
        {
            int choice = Utils.Utils.CustomMessageBox(new[] { AppResources.Allow, AppResources.Cancel },
                    AppResources.UseLocationTitle,
                    AppResources.UseLocationContent);

            if (choice == 0)
            {
                RememberConnection.SaveAccessLocation(true);
                LaunchLocalization();
                return 1;
            }
            return 0;
        }

        private void PMMapView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (RememberConnection.IsFirstConnection() == true)
            {
                AccessLocationMsgBox();
            }
            RememberConnection.SetFirstConnection();
        }

        ////////////////////////////////////////////////    Middle Page    /////////////////////////////////////////////

        private void MenuDown_OnClick(object sender, RoutedEventArgs e)
        {
            _currentView = CurrentMapPageView.UnderMenuView;
            _isUnderMenuOpen = true;
            _enableSwipe = false;

            if (sender.Equals(NotificationButton))
            {
                DownMenuTitle.Text = AppResources.Notifications;
            }
            else if (sender.Equals(ContactsButton))
            {
                DownMenuTitle.Text = AppResources.Contacts;
            }

            MainGridMap.RowDefinitions[2].Height = new GridLength(0);
            MoveAnimationUp.Begin();
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
                if (_geoLocation == null)
                {
                    AccessLocationMsgBox();
                }
                else if (_geoLocation.GeopositionUser != null)
                {
                    Map.Center = new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude);
                }
            });  
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            int retValue = Utils.Utils.CustomMessageBox(new[] {"Yes", "No"}, AppResources.QuitAppTitle, AppResources.QuitApp);

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

        private void Center_Click(object sender, RoutedEventArgs e)
        {
            UpdateMapCenter();
        }

        private void Map_OnTouch(object sender, RoutedEventArgs e)
        {
            if (_isUnderMenuOpen == true)
            {
                MainGridMap.RowDefinitions[2].Height = new GridLength(120);
           
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
            try
            {
                NavigationService.Navigate(Paths.SettingsView);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }

        private void ButtonReward_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            int choice = Utils.Utils.CustomMessageBox(new[] {AppResources.Yes, AppResources.No}, AppResources.MenuLogout, AppResources.LogoutSentence);

            if (choice == 0)
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


        ////////////////////////////////////////////////    Down Menu    /////////////////////////////////////////////

        private void CloseMenuDownButton_Click(object sender, RoutedEventArgs e)
        {
            Map_OnTouch(sender, e);
        }

        ////////////////////////////////////////////////    Right Menu    /////////////////////////////////////////////

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool NotifyPropertyChanged<T>(T variable, T valeur, [CallerMemberName] string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) return false;

            variable = valeur;
            NotifyPropertyChanged(nomPropriete);
            return true;
        }

        public IEnumerable ListPickerPins
        {
            get { return listPickerPins; }
            set { NotifyPropertyChanged(listPickerPins, value); }
        }

        public class ElementsListPicker
        {
            public string Code {get; set; }
            public string Name {get; set; }
            public Uri ImgPath { get; set; }
        }

        private IEnumerable listPickerPins { get; set; }

        private void ListPickerPinType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPickerPinType.SelectedIndex == ((int)PMPinModel.PinsType.PrivateMessage))
            {
                GridNewPinPivotItem.RowDefinitions[2].Height = GridNewPinPivotItem.RowDefinitions[3].Height;
            }
            else if (ListPickerPinType.SelectedIndex == ((int) PMPinModel.PinsType.Event))
            {
                GridNewPinPivotItem.RowDefinitions[1].Height = GridNewPinPivotItem.RowDefinitions[3].Height;
            }
            else
            {
                GridNewPinPivotItem.RowDefinitions[2].Height = new GridLength(0);
            }
        }

        private void PostPinButton_ClickPreJob()
        {
            PostPinProgressBar.Visibility = Visibility.Visible;
            PostPinProgressBar.IsIndeterminate = true;
            NewPinPivotItem.IsEnabled = false;
        }

        private void PostPinButton_ClickPostJob()
        {
            PostPinProgressBar.Visibility = Visibility.Collapsed;
            PostPinProgressBar.IsIndeterminate = false;
            NewPinPivotItem.IsEnabled = true;
        }

        private void PostPinButton_Click(object sender, RoutedEventArgs e)
        {
            if (_geoLocation == null)
                if (AccessLocationMsgBox() != 1)
                    return;
            if (_geoLocation != null && _geoLocation.GeopositionUser == null)
                return;
           
            var pc = new PMPinController(RequestType.CreatePin, PostPinButton_ClickPostJob);

            PostPinButton_ClickPreJob();

            switch (ListPickerPinType.SelectedIndex)
            {
                case ((int)PMPinModel.PinsType.PublicMessage):
                    pc.CreatePin(_geoLocation.GeopositionUser, new[] { PinTitle.Text, PinContent.Text, ((int)PMPinModel.PinsType.PublicMessage).ToString() });
                    break;

                case ((int)PMPinModel.PinsType.PrivateMessage):
                    pc.CreatePin(_geoLocation.GeopositionUser, new[] { PinTitle.Text, PinContent.Text, ((int)PMPinModel.PinsType.PrivateMessage).ToString() });
                    break;

                case ((int)PMPinModel.PinsType.Event):
                    pc.CreatePin(_geoLocation.GeopositionUser, new[] { PinTitle.Text, PinContent.Text, ((int)PMPinModel.PinsType.Event).ToString() });
                    break;

                case ((int)PMPinModel.PinsType.Eye):
                    pc.CreatePin(_geoLocation.GeopositionUser, new[] { PinTitle.Text, PinContent.Text, ((int)PMPinModel.PinsType.Eye).ToString() });
                    break;

                case ((int)PMPinModel.PinsType.PointOfInterest):
                    pc.CreatePin(_geoLocation.GeopositionUser, new[] { PinTitle.Text, PinContent.Text, ((int)PMPinModel.PinsType.PointOfInterest).ToString() });
                    break;

                default:
                    Logs.Output.ShowOutput("rien du tout");
                    break;
            }
       
            string json = @"[
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

        private void PinTitle_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PinContent.Focus();
            }
        }

        private void PinReceiver_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PinTitle.Focus();
            }
        }

    }
}