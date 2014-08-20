using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Model;
using PinMessaging.Controller;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace PinMessaging.View
{
    public partial class PMMapView : PhoneApplicationPage
    {
        private enum CurrentMapPageView
        {
            MapView,
            LeftMenuView,
            RightMenuView,
            UnderMenuView
        }

        private static CurrentMapPageView _currentView = CurrentMapPageView.MapView;
        private static bool _isUnderMenuOpen = false;
        private static readonly PMPinModel PinCreateModel = new PMPinModel();
        private PMPinModel _currentPinFocused;

        private readonly MapLayer _mapLayer = new MapLayer();
        private readonly MapOverlay _userSpotLayer = new MapOverlay();
        private readonly UserLocationMarker _userSpot = new UserLocationMarker();
        public PMGeoLocation _geoLocation = null;
        private readonly BackgroundWorker _bkw = new BackgroundWorker();
        private readonly DispatcherTimer _searchContactTimer = new DispatcherTimer();
        private NotificationCenter _notificationCenter = new NotificationCenter();


        public PMMapView()
        {
            InitializeComponent();

            CreateAppBarMap();

            _searchContactTimer.Tick += dispatcherTimer_Tick;
            _searchContactTimer.Interval = new TimeSpan(0, 0, 1);

            _bkw.WorkerReportsProgress = true;
            _bkw.DoWork += BkwOnDoWork;
            _bkw.ProgressChanged += BkwOnProgressChanged;

            //central page
            ImgTarget.ImageSource = new BitmapImage(Paths.TargetButton);

            try
            {
                (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Text = AppResources.RefreshPins;
                (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).Text = AppResources.CreatePinTitle;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("Loading ApplicationBar text error", exp, Logs.Error.ErrorsPriority.NotCritical);
            }


            //left menu
            ImgMap.Source = new BitmapImage(Paths.LeftMenuMap);
            ImgFilters.Source = new BitmapImage(Paths.LeftMenuFilters);
            ImgProfil.Source = new BitmapImage(Paths.LeftMenuProfil);
            ImgPins.Source = new BitmapImage(Paths.LeftMenuPins);
            ImgSettings.Source = new BitmapImage(Paths.LeftMenuSettings);
            ImgAbout.Source = new BitmapImage(Paths.LeftMenuAbout);
            ImgReward.Source = new BitmapImage(Paths.LeftMenuRewards);
            ImgLogout.Source = new BitmapImage(Paths.LeftMenuLogout);

            bool? accessLoc = RememberConnection.GetAccessLocation();
            Logs.Output.ShowOutput(accessLoc.ToString());

            if (accessLoc == true)
            {
                Logs.Output.ShowOutput("accesloc true");
                LaunchLocalization();
            }

            _notificationCenter.Init(this);
            PMMapPinController.Init(this);
            PMMapContactController.Init(this);
            LoadRessources();

            if (PMData.AppMode == PMData.ApplicationMode.Offline)
            {
                //PostPinButton.IsEnabled = false;
            }

            PMData.LoadPins();
            PMData.LoadFavorites();
            PMData.LoadProfilPictures();
            ResetCreatePinModel();

            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPublicMessage,
                Image = new BitmapImage(Paths.PinPublicMessageIconIntermediate),
                PinType = PMPinModel.PinsType.Message
            });
            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPublicEvent,
                Image = new BitmapImage(Paths.PinEventIconIntermediate),
                PinType = PMPinModel.PinsType.Event
            });
            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPublicPointOfView,
                Image = new BitmapImage(Paths.PinViewIconIntermediate),
                PinType = PMPinModel.PinsType.View
            });
            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPrivateMessage,
                Image = new BitmapImage(Paths.PinPrivateMessageIconIntermediate),
                PinType = PMPinModel.PinsType.PrivateMessage
            });
            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPrivateEvent,
                Image = new BitmapImage(Paths.PinPrivateEventIconIntermediate),
                PinType = PMPinModel.PinsType.PrivateEvent
            });
            PinListPicker.Items.Add(new PinItem
            {
                Name = AppResources.PinPrivatePointOfView,
                Image = new BitmapImage(Paths.PinPrivateViewIconIntermediate),
                PinType = PMPinModel.PinsType.PrivateView
            });
        }

        private class PinItem
        {
            public string Name { get; set; }
            public BitmapImage Image { get; set; }
            public PMPinModel.PinsType PinType { get; set; }
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
            catch (Exception exp)
            {
                Logs.Error.ShowError("LaunchLocalization", exp, Logs.Error.ErrorsPriority.NotCritical);
            }

            PMData.MapLayerContainer = _mapLayer;

            UpdateLocationUi();
        }

        private int AccessLocationMsgBox()
        {
            int choice = Utils.Utils.CustomMessageBox(new[] {AppResources.Allow, AppResources.Cancel},
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Map.ZoomLevel -= 1;
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

        private void AdaptUiUnderMenuClick(RowDefinition exept, bool pinDescFull, bool contactFull, bool pinCreateFull)
        {
            for (var i = 2; i < UnderMenuGrid.RowDefinitions.Count; i++)
            {
                UnderMenuGrid.RowDefinitions[i].Height = UnderMenuGrid.RowDefinitions[i].Equals(exept) == false
                    ? new GridLength(0)
                    : new GridLength(1, GridUnitType.Star);
            }
            UnderMenuPinDescriptionGrid.Height = (pinDescFull == true ? UnderMenuPinDescriptionScrollView.Height : 0);
            UnderMenuContactPanel.Height = (contactFull == true ? UnderMenuContactScrollViewer.Height : 0);
            UnderMenuCreatePinGrid.Height = (pinCreateFull == true ? UnderMenuCreatePinScrollViewer.Height : 0);
        }

        private void MenuDown_PinOnClick()
        {
            Dispatcher.BeginInvoke(() =>
            {
                _currentView = CurrentMapPageView.UnderMenuView;
                _isUnderMenuOpen = true;
                _enableSwipe = false;
                ApplicationBar.IsVisible = false;

                //DownMenuTitle.Text = AppResources.Pin;
                AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[4], true, false, false);

                MainGridMap.RowDefinitions[2].Height = new GridLength(0);
                MoveAnimationUp.Begin();
            });
        }

        private void MenuDown_NotificationOnClick()
        {
            DownMenuTitle.Text = AppResources.Notifications;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[3], false, false, false);
        }

        private void MenuDown_ContactOnClick()
        {
            DownMenuTitle.Text = AppResources.Contacts;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[2], false, true, false);
        }

        private void MenuDown_CreatePin()
        {
            DownMenuTitle.Text = AppResources.CreatePinTitle;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[5], false, false, true);
        }

        private void MenuDown_CommonActionsBefore()
        {
            _currentView = CurrentMapPageView.UnderMenuView;
            _isUnderMenuOpen = true;
            _enableSwipe = false;
            ApplicationBar.IsVisible = false;
        }

        private void MenuDown_CommonActionsAfter()
        {
            MainGridMap.RowDefinitions[2].Height = new GridLength(0);
            MoveAnimationUp.Begin();
        }

        private void ApplicationBarMenuItemCreate_OnClick(object sender, EventArgs e)
        {
            MenuDown_CommonActionsBefore();
            MenuDown_CreatePin();
            MenuDown_CommonActionsAfter();
        }

        private void ApplicationBarMenuItemRefresh_OnClick(object sender, EventArgs e)
        {
            RefreshPinButton_OnClick(null, null);
        }

        private void MenuDownNotification_OnClick(object sender, EventArgs e)
        {
            MenuDown_CommonActionsBefore();
            MenuDown_NotificationOnClick();
            MenuDown_CommonActionsAfter();
        }

        private void MenuDownContacts_OnClick(object sender, EventArgs e)
        {
            MenuDown_CommonActionsBefore();
            MenuDown_ContactOnClick();
            MenuDown_CommonActionsAfter();
        }

        private void MenuDown_OnClick(object sender, EventArgs eventArgs)
        {
            if (sender == null)
            {
                Logs.Error.ShowError("MenuDown_OnClick: sender is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            if (sender.Equals(ButtonPins))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_PinOnClick();
                MenuDown_CommonActionsAfter();
            }
            else if (sender.Equals(CreatePinButton))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_CreatePin();
                MenuDown_CommonActionsAfter();
            }
        }

        public void UpdateLocationUi()
        {
            if (_geoLocation == null)
            {
                Logs.Error.ShowError("UpdateLocationUi: _geoLocation is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            } 

            _geoLocation.UpdateLocation();

            if (_geoLocation.GeopositionUser == null)
            {
                Logs.Error.ShowError("UpdateLocationUi: GeopositionUser is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (_userSpot.Visibility == Visibility.Collapsed)
                    _userSpot.Visibility = Visibility.Visible;
                _userSpotLayer.GeoCoordinate = new GeoCoordinate(_geoLocation.GeopositionUser.Coordinate.Latitude, _geoLocation.GeopositionUser.Coordinate.Longitude);
            });
        }

        private void ZoomTo(double zoomLevel)
        {
            if (zoomLevel < 1 || zoomLevel > 20 || Map.ZoomLevel > zoomLevel)
                return;

            _bkw.RunWorkerAsync(new Tuple<double, double>(Map.ZoomLevel, zoomLevel));
        }

        private void BkwOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var tuple = (Tuple<double, double>) doWorkEventArgs.Argument;
            var fromMapZoomLevel = tuple.Item1;
            var toZoomLevel = tuple.Item2;

            while (fromMapZoomLevel < toZoomLevel)
            {
                fromMapZoomLevel += 0.10d;

                Thread.Sleep(10);
                _bkw.ReportProgress(2);
            }
            _bkw.ReportProgress(100);
        }

        private void BkwOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            if (Map.ZoomLevel + 0.10d < 20 && Map.ZoomLevel - 0.10d > 1)
            {
                Map.ZoomLevel += 0.10d;
                UpdateMapCenter();
            }
        }

        public void MapCenterOn(GeoCoordinate pos)
        {
            if (pos == null)
            {
                Logs.Error.ShowError("MapCenterOn: pos is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            Dispatcher.BeginInvoke(() =>
            {
                Map.Center = pos;
            });
            ZoomTo(17);
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

        protected override async void OnBackKeyPress(CancelEventArgs e)
        {
            if (e == null)
            {
                Logs.Error.ShowError("OnBackKeyPress: e is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            switch (_currentView)
            {
                case CurrentMapPageView.LeftMenuView:
                    OpenClose_Left(null, null);
                    e.Cancel = true;
                    return;

                case CurrentMapPageView.RightMenuView:
                    OpenClose_Right(null, null);
                    e.Cancel = true;
                    return;

                case CurrentMapPageView.UnderMenuView:
                    CloseMenuDownButton_Click(null, null);
                    e.Cancel = true;
                    return;
            }

            int retValue = Utils.Utils.CustomMessageBox(new[] {"Yes", "No"}, AppResources.QuitAppTitle,
                AppResources.QuitApp);

            //0 is Yes, 1 is No....
            if (retValue == 0)
            {
                bool retPins = await PMData.SaveData(PMData.StoredDataType.Pins);
                bool retFav = await PMData.SaveData(PMData.StoredDataType.Favorites);
                bool retPic = await PMData.SaveData(PMData.StoredDataType.Pictures);

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

        private void RefreshPinButton_PostClick()
        {
            ProgressBarActive(false);

            //!/ MAYBE REMOVE THAT
            LoadMyPinsFromUserList();
        }

        private void RefreshPinButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_geoLocation == null || _geoLocation.GeopositionUser == null)
            {
                Logs.Error.ShowError("RefreshPinButton_OnClick: " + (_geoLocation == null ? "_gelocation" : "GeopositionUser") + " is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            ProgressBarActive(true);

            var pinController = new PMPinController(RequestType.GetPins, RefreshPinButton_PostClick);

            pinController.GetPins(
                Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Latitude.ToString()),
                Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Longitude.ToString()));
        }

        private void Map_OnTouch(object sender, RoutedEventArgs e)
        {
            if (_isUnderMenuOpen == true)
            {
                _currentView = CurrentMapPageView.MapView;
                _isUnderMenuOpen = false;
                _enableSwipe = true;
                MoveAnimationDown.Begin();
                ApplicationBar.IsVisible = true;
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

        ///////////////////////////////////     MANAGE SWIPE LEFT RIGHT        //////////////////////////////////

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
                _leftPage = -(double) Application.Current.Resources["AdditionalMapMenuWidth"];
                _rightPage = -2*(double) Application.Current.Resources["AdditionalMapMenuWidth"];
            }
            catch (Exception)
            {
                Logs.Error.ShowError("Could not load AdditionalMapMenuWidth from App.xaml", Logs.Error.ErrorsPriority.NotCritical);
                _leftPage = -420;
                _rightPage = -840;
            }
        }

        private void OpenClose_Left(object sender, EventArgs eventArgs)
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

        private void Open_Right(object sender, EventArgs e)
        {
            OpenClose_Right(sender, e);
            LoadMyPins();
        }

        private void OpenClose_Right(object sender, EventArgs e)
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

        private void MoveViewWindow(double left)
        {
            _viewMoved = true;

            if (left == 0 || left == -840)
            {
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
                Map.IsEnabled = false;
            }
            else
            {
                _currentView = CurrentMapPageView.MapView;
                ApplicationBar.Mode = ApplicationBarMode.Default;
                Map.IsEnabled = true;
            }

            moveAnimation.SkipToFill();
            ((DoubleAnimation) (moveAnimation).Children[0]).To = left;
            moveAnimation.Begin();
        }

        private void canvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e == null)
            {
                Logs.Error.ShowError("RefreshPinButton_OnClick: e is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

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
            ContactNameOnTapSub(PMData.CurrentUserId);
        }

        private void ButtonPins_OnClick(object sender, RoutedEventArgs e)
        {
            OpenClose_Left(null, null);
            Open_Right(null, null);
        }

        private void ButtonFilters_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(Paths.FilterView);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
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
            var userController = new PMUserController(RequestType.User, ButtonReward_PostClick);

            userController.GetUserInfos(PMData.CurrentUserId);
        }

        private void ButtonReward_PostClick()
        {
            //open the pivot "badges" in the currentUserProfil view
            NavigationService.Navigate(new Uri(Paths.CurrentUserProfilPathString + "?open=2", UriKind.Relative));
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            //open the pivot "about" in the setting view
            NavigationService.Navigate(new Uri(Paths.SettingsPathString + "?open=2", UriKind.Relative));
        }

        private void ButtonLogout_OnClick(object sender, RoutedEventArgs e)
        {
            var choice = Utils.Utils.CustomMessageBox(new[] {AppResources.Yes, AppResources.No}, AppResources.MenuLogout, AppResources.LogoutSentence);

            if (choice != 0)
                return;

            try
            {
                NavigationService.Navigate(Paths.FirstLaunch);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }

        /////////////////////////////////////////////////   RIGHT MENU ///////////////////////////////////////////////
        
        private void MyPinItemUi(PMPinModel pin)
        {
            var historyImage = new Image
            {
                Height = 70,
                Width = 70,
                Source = Paths.PinsMapImg[pin.PinType + (pin.Private == true ? 6 : 0)]
            };
            var titleTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 30,
                VerticalAlignment = VerticalAlignment.Center,
                Text = pin.Title,
                Height = 40
            };
            var messageTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                TextAlignment = TextAlignment.Left,
                Text = pin.Content,
                Height = 25
            };
            var dateTextBlock = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                TextAlignment = TextAlignment.Left,
                Height = 25
            };

            var res = Utils.Utils.ConvertStringToDouble(pin.CreatedTime);

            if (res != null)
            {
                var d = Utils.Utils.ConvertFromUnixTimestamp(res);
                dateTextBlock.Text = d.ToShortDateString() + " " + d.ToShortTimeString();
            }

            var itemMyPin = new Button
            {
                Margin = new Thickness(-20, 0, 0, 0),
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            itemMyPin.Click += ItemMyPinOnClick;

            var itemMainGrid = new Grid() {HorizontalAlignment = HorizontalAlignment.Left};
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(70)});
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            itemMainGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(70)});
            itemMainGrid.Tag = pin;

            itemMyPin.Content = itemMainGrid;

            MyPinsStackPanel.Children.Add(itemMyPin);

            itemMainGrid.Children.Add(historyImage);

            var pinContentStackPanel = new StackPanel
            {
                Margin = new Thickness(20, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            pinContentStackPanel.Children.Add(titleTextBlock);
            pinContentStackPanel.Children.Add(messageTextBlock);
            pinContentStackPanel.Children.Add(dateTextBlock);

            itemMainGrid.Children.Add(pinContentStackPanel);

            Grid.SetRow(historyImage, 0);
            Grid.SetColumn(historyImage, 0);

            Grid.SetRow(pinContentStackPanel, 0);
            Grid.SetColumn(pinContentStackPanel, 1);
        }

        private void ItemMyPinOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                var itemButton = sender as Button;

                if (itemButton == null)
                {
                    Logs.Error.ShowError("ItemMyPinOnClick: itemButton is null", Logs.Error.ErrorsPriority.NotCritical);
                    return;
                }

                var itemGrid = itemButton.Content as Grid;

                if (itemGrid == null)
                {
                    Logs.Error.ShowError("ItemMyPinOnClick: itemGrid is null", Logs.Error.ErrorsPriority.NotCritical);
                    return;
                }

                var pin = (PMPinModel)itemGrid.Tag;

                if (pin == null)
                {
                    Logs.Error.ShowError("ItemMyPinOnClick: pin is null", Logs.Error.ErrorsPriority.NotCritical);
                    return;
                }

                OpenClose_Right(null, null);
                PinTapped(pin);
                MapCenterOn(pin.GeoCoord);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ItemMyPinOnClick: error when getting the pin info: ", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void LoadMyPinsFromUserList()
        {
            foreach (var pin in PMData.PinsList)
            {
                AddPinToMyPinsUi(pin);
            }
        }

        private void AddPinToMyPinsUi(PMPinModel pin)
        {
            try
            {
                //Make sure each element in the stackpanel is a Grid with a pin in Tag otherwise exception
                if (pin.AuthorId == PMData.CurrentUserId && MyPinsStackPanel.Children.Any(elem => (((((elem as Button).Content as Grid).Tag) as PMPinModel).Id) == pin.Id) == false)
                    MyPinItemUi(pin);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("AddPinToMyPinsUi: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void LoadMyPins()
        {
            //if (MyPinsStackPanel.Children.Count == 0)
            LoadMyPinsFromUserList();
        }

        private void PivotPins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotPins.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    break;
            }
        }

        ////////////////////////////////////////////////    Down Menu    /////////////////////////////////////////////

        public void CloseMenuDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationBar.IsVisible == false)
                ApplicationBar.IsVisible = true;
            LockUnlockCommentCheck(true);

            Map_OnTouch(sender, e);
        }

        /// ///////////////////////////////////////////    Contact    //////////////////////////////////////////////////

        private void SearchContactsTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_searchContactTimer.IsEnabled == true)
                _searchContactTimer.Stop();
            _searchContactTimer.Start();
        }

        private void SearchContactUpdateUi()
        {
            Dispatcher.BeginInvoke(() =>
            {
                UnderMenuProgressBar.IsIndeterminate = false;
                UnderMenuProgressBar.Visibility = Visibility.Collapsed;
                SearchContactsTextBox.IsEnabled = true;
                SearchContactStackPanel.Children.Clear();

                if (PMData.SearchUserList == null)
                {
                    Logs.Error.ShowError("SearchContactUpdateUi: SearchUserList is null", Logs.Error.ErrorsPriority.NotCritical);
                    return;
                }

                if (PMData.SearchUserList.Count == 0)
                {
                    SearchContactStackPanel.Children.Add(new TextBlock
                    {
                        Height = 80,
                        Width = 300,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Text = AppResources.NoResults,
                        Margin = new Thickness(0, 0, 0, 0),
                        FontSize = 25
                    });
                }
                else
                {
                    foreach (var user in PMData.SearchUserList)
                    {
                        AddContactUi(user, SearchContactStackPanel);
                    }
                }
            });
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (SearchContactsTextBox.Text.Equals(""))
                return;

            if (sender == null)
            {
                Logs.Error.ShowError("dispatcherTimer_Tick: sender is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var distpach = sender as DispatcherTimer;

            if (distpach == null)
            {
                Logs.Error.ShowError("dispatcherTimer_Tick: distpach is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            distpach.Stop();

            SearchContactsTextBox.IsEnabled = false;
            UnderMenuProgressBar.IsIndeterminate = true;
            UnderMenuProgressBar.Visibility = Visibility.Visible;

            var uc = new PMUserController(RequestType.SearchUser, SearchContactUpdateUi);
            uc.SearchUser(SearchContactsTextBox.Text);
        }

        private void ContactNameOnTapSub(string userId)
        {
            if (userId != null)
            {
                var userController = new PMUserController(RequestType.User, PinAuthorDescriptionTextBlock_PostTap);

                userController.GetUserInfos(userId);
            }
            else
            {
                Logs.Error.ShowError("ContactNameOnTap: could not get the info about the author", Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void ContactNameOnTap(object sender, GestureEventArgs gestureEventArgs)
        {
            var img = sender as Image;
             
            if (img == null)
            {
                var tb = sender as TextBlock;

                if (tb != null)
                {
                    var user = tb.Tag as PMUserModel;

                    if (user != null)
                        ContactNameOnTapSub(user.Id);
                }
            }
            else
            {
                var user = img.Tag as PMUserModel;

                if (user != null)
                    ContactNameOnTapSub(user.Id);
            }
        }

        private void AddContactUi(PMUserModel user, StackPanel where)
        {
            if (user == null)
            {
                Logs.Error.ShowError("AddContactUi: user is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var contactGrid = new Grid {Tag = user};

            contactGrid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(110)});

            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(110)});
            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});

            var contactImg = new Image()
            {
                Name = where.Name + "contactImg" + user.Id,
                Source = new BitmapImage(new Uri("/Images/Icons/neutral_profil.jpg", UriKind.Relative)),
                Tag = user
            };
            var contactName = new TextBlock()
            {
                Text = user.Pseudo,
                Tag = user,
                FontSize = 25,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20, 0, 0, 0)
            };

            Logs.Output.ShowOutput(where.Name + "contactImg" + user.Id);

            contactImg.Tap += ContactNameOnTap;
            contactName.Tap += ContactNameOnTap;

            contactGrid.Children.Add(contactImg);
            contactGrid.Children.Add(contactName);

            Grid.SetRow(contactImg, 0);
            Grid.SetColumn(contactImg, 0);

            Grid.SetRow(contactName, 0);
            Grid.SetColumn(contactName, 1);

            where.Children.Add(contactGrid);

            PMData.UserId = user.Id;

            var userController = new PMUserController(RequestType.ProfilPicture,
                where.Equals(SearchContactStackPanel)
                    ? (Action) SearchContactPictureUpdateUi
                    : (Action) ContactPictureUpdateUi);
            userController.DownloadProfilPicture(user.Id);
        }

        private void ContactPictureUpdateUi()
        {
            Design.ProfilPictureUpdateUi(AuthorPicture, PMData.UserId);
        }

        private static int _gridNbr = 0;
        private void SearchContactPictureUpdateUi()
        {
            if (_gridNbr > SearchContactStackPanel.Children.Count - 1)
            {
                Logs.Error.ShowError("SearchContactPictureUpdateUi: gridNbr is invalid", Logs.Error.ErrorsPriority.NotCritical);
                return; 
            }

            var grid = SearchContactStackPanel.Children[_gridNbr] as Grid;

            _gridNbr++;
            if (_gridNbr > SearchContactStackPanel.Children.Count - 1)
                _gridNbr = 0;

            if (grid == null)
            {
                Logs.Error.ShowError("SearchContactPictureUpdateUi: grid is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var user = grid.Tag as PMUserModel;

            if (user == null)
            {
                Logs.Error.ShowError("SearchContactPictureUpdateUi: user is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var img = grid.FindName(SearchContactStackPanel.Name + "contactImg" + user.Id);
            Logs.Output.ShowOutput(SearchContactStackPanel.Name + "contactImg" + user.Id);

            if (img == null)
            {
                Logs.Output.ShowOutput("NOOOOOOOOOOOOOOO");
            }
            else
            {
                Design.ProfilPictureUpdateUi((Image)img, PMData.UserId);
                Logs.Output.ShowOutput("YESSSSSSSSSS");
            }
        }

        private static void AddContactCode(PMUserModel user)
        {
            PMData.AddToQueueUserList(user);
        }

        public void AddContact(PMUserModel user)
        {
            if (PMMapContactController.IsFavoriteUnique(user) == true)
            {
                AddContactUi(user, UnderMenuContactPanel);
                AddContactCode(user);
            }
        }

        private void RemoveContactUi(PMUserModel user)
        {
            if (UnderMenuContactPanel.Children.Count == 0)
            {
                Logs.Error.ShowError("RemoveContactUi: UnderMenuContactPanel.Children.Count = 0", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            try
            {
                var rep = UnderMenuContactPanel.Children.First(contact =>
                {
                    var grid = contact as Grid;

                    return grid != null && (((grid.Children[0]) as Image).Tag as PMUserModel).Id == user.Id;
                });

                if (rep != null)
                {
                    Logs.Output.ShowOutput((rep as Grid).Children.Count.ToString());
                    UnderMenuContactPanel.Children.Remove(rep);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("RemoveContactUI: ", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private static void RemoveContactCode(PMUserModel user)
        {
            PMData.RemoveToUserList(user);
        }

        public void RemoveContact(PMUserModel user)
        {
            RemoveContactUi(user);
            RemoveContactCode(user);
        }

        /// //////////////////////////////////////////     Pin description      ////////////////////////////////////////

        private void AddCommentsToUi()
        {
            //the first comment displayed is the most recent
            PMData.PinsCommentsListTmp = (from elem in PMData.PinsCommentsListTmp orderby elem.CreatedTime ascending select elem).ToList();

            foreach (var tb in PMData.PinsCommentsListTmp)
            {
                var chatBubble = new ChatBubble
                {
                    Background = new SolidColorBrush(Colors.DarkGray),
                    BorderBrush = new SolidColorBrush(Colors.DarkGray),
                    ChatBubbleDirection = ChatBubbleDirection.LowerLeft,
                    Content = new TextBlock()
                    {
                        Text = tb.Content,
                        FontSize = 20,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = new SolidColorBrush(Colors.Black)
                    },
                };

                CommentStackPanel.Children.Insert(0, chatBubble);

                var authorName = new TextBlock()
                {
                    Text = tb.Author,
                    FontSize = 25,
                    Foreground = (Brush) Application.Current.Resources["PMOrange"],
                    TextAlignment = TextAlignment.Left,
                    Tag = tb.AuthorId
                };
                authorName.Tap += AuthorNameOnTap;

                var creationDate = new TextBlock()
                {
                    FontSize = 20,
                    Foreground = new SolidColorBrush(Colors.DarkGray),
                    TextAlignment = TextAlignment.Right
                };

                var res = Utils.Utils.ConvertStringToDouble(tb.CreatedTime);

                if (res != null)
                {
                    var d = Utils.Utils.ConvertFromUnixTimestamp(res);
                    creationDate.Text = d.ToShortDateString() + " " + d.ToShortTimeString();
                }

                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                grid.Children.Add(authorName);
                grid.Children.Add(creationDate);

                Grid.SetColumn(authorName, 0);
                Grid.SetColumn(creationDate, 1);

                CommentStackPanel.Children.Insert(1, grid);
            }

            PMData.AddToQueuePinComments(PMData.PinsCommentsListTmp);
            PMData.PinsCommentsListTmp.Clear();
        }

        private void AuthorNameOnTap(object sender, GestureEventArgs gestureEventArgs)
        {
            var tb = sender as TextBlock;

            if (tb == null)
            {
                Logs.Error.ShowError("AuthorNameOnTap: tb is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var userId = tb.Tag as string;

            if (userId == null)
            {
                Logs.Error.ShowError("AuthorNameOnTap: userId is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            ContactNameOnTapSub(userId);
        }

        private static string GetPinTypeName(PMPinModel.PinsType type, bool isPrivate)
        {
            switch (type)
            {
                case PMPinModel.PinsType.Message:
                    return isPrivate ? AppResources.PinPrivateMessage : AppResources.PinPublicMessage;

                case PMPinModel.PinsType.Event:
                    return isPrivate ? AppResources.PinPrivateEvent : AppResources.PinPublicEvent;

                case PMPinModel.PinsType.View:
                    return isPrivate ? AppResources.PinPrivatePointOfView : AppResources.PinPublicPointOfView;

                default:
                    return AppResources.PinPublicMessage;
            }
        }

        public void PinTapped(PMPinModel pin)
        {
            if (pin == null)
            {
                Logs.Error.ShowError("PinTapped: pin is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            CommentStackPanel.Children.Clear();

            var pinC = new PMPinController(RequestType.GetPinMessages, GetPinMessages_Post);

            pinC.GetPinMessage(pin);

            var dateStr = ((pin.PinType == PMPinModel.PinsType.Event) && (pin.Date != null)
                ? Environment.NewLine + "L'évènement se déroule le " +
                  pin.Date.ToLongDateString() + " à " +
                  pin.Date.ToLongTimeString()
                : "");

            PinMessageDescriptionTextBlock.Text = pin.Content + dateStr;
            PinAuthorDescriptionTextBlock.Text = pin.Author;
            PinAuthorDescriptionTextBlock.Tag = pin;

            PinTitleTextBlock.Text = GetPinTypeName(pin.PinType, pin.Private);

            PinDescriptionImage.Source = Paths.PinsMapImg[pin.PinType + (pin.Private == true ? 6 : 0)];

            DownMenuTitle.Text = pin.Title;

            var res = Utils.Utils.ConvertStringToDouble(pin.CreatedTime);

            if (res != null)
            {
                var d = Utils.Utils.ConvertFromUnixTimestamp(res);
                PinCreationTimeDescriptionTextBlock.Text = d.ToLongDateString();
            }

            if (Design.ProfilPictureUpdateUi(AuthorPicture, pin.AuthorId) == false)
            {
                AuthorPicture.Source = new BitmapImage(Paths.NeutralProfilPicture);
                PMData.UserId = pin.AuthorId;
                var userController = new PMUserController(RequestType.ProfilPicture, ProfilPictureUpdateUi);
                userController.DownloadProfilPicture(pin.AuthorId);
            }

            if (pin.Url != null)
            {
                UnderMenuPinDescriptionGrid.RowDefinitions[2].Height = new GridLength(300);
                PinImage.Source = PMWebService.DownloadImageUrl(pin.Url);
            }
            else
            {
                UnderMenuPinDescriptionGrid.RowDefinitions[2].Height = new GridLength(0);
            }

            _currentPinFocused = pin;

            MenuDown_OnClick(ButtonPins, new RoutedEventArgs());
        }

        private void ProfilPictureUpdateUi()
        {
            Design.ProfilPictureUpdateUi(AuthorPicture, PMData.UserId);
        }

        public void GetPinMessages_Post()
        {
            CommentStackPanel.Children.Clear();
            AddCommentsToUi();
        }

        private void PinAuthorDescriptionTextBlock_PostTap()
        {
            PinAuthorDescriptionLock(false);

            try
            {
                if (PMData.User != null)
                    NavigationService.Navigate(PMData.User.Id != PMData.CurrentUserId ? Paths.UserProfilView : Paths.CurrentUserProfilView);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }

        private void PinAuthorDescriptionLock(bool lockStatus)
        {
            UnderMenuProgressBar.IsIndeterminate = lockStatus;
            UnderMenuProgressBar.Visibility = (lockStatus ? Visibility.Visible : Visibility.Collapsed);
        }

        private void PinAuthorDescriptionTextBlock_OnTap(object sender, GestureEventArgs e)
        {
            var pin = PinAuthorDescriptionTextBlock.Tag as PMPinModel;

            if (pin != null)
            {
                PinAuthorDescriptionLock(true);

                var userController = new PMUserController(RequestType.User, PinAuthorDescriptionTextBlock_PostTap);

                userController.GetUserInfos(pin.AuthorId);
            }
            else
            {
                Logs.Error.ShowError("PinAuthorDescriptionTextBlock_OnTap: could not get the info about the author", Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        ////////////////////////////////////////////////    Create pin         //////////////////////////////////////////

        public void DropPrivatePin(PMUserModel user)
        {
            PinListPicker.SelectedIndex = 3;
            TargetLongListSelector.Height = new GridLength(100).Value;
            NoContactsTextBlock.Visibility = Visibility.Collapsed;
            TargetLongListSelector.ItemsSource = new List<PMUserModel> {user};

            ApplicationBarMenuItemCreate_OnClick(null, null);
        }

        private void MoveAnimationUp_OnCompleted(object sender, EventArgs e)
        {
            //from DropPrivatePin
            if (TargetLongListSelector.ItemsSource != null && TargetLongListSelector.ItemsSource.Count > 0)
                return;

            if (PMData.UserList.Count > 0)
                TargetLongListSelector.ItemsSource = PMData.UserList;

            NoContactsTextBlock.Visibility = PMData.UserList.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            TargetLongListSelector.Height = TargetLongListSelector.ItemsSource == null ? TargetLongListSelector.Height = 0 : new GridLength(100).Value;
           
            CheckCanCreatePin();
        }

        private void CheckCanCreatePin()
        {
            bool canCreate = false;

            if (PinCreateModel.Content.Length > 0 && PinCreateModel.Title.Length > 0)
            {
                if (PinCreateModel.Private == true)
                {
                    if (PinCreateModel.AuthoriseUsersId.Equals("") == false)
                    {
                        canCreate = true;
                    }
                }
                else
                {
                    canCreate = true;
                }
            }

            DropPinButton.IsEnabled = canCreate == true;
        }

        private static void ResetCreatePinModel()
        {
            PinCreateModel.Id = string.Empty;
            PinCreateModel.Lang = string.Empty;
            PinCreateModel.PinType = PMPinModel.PinsType.Message;
            PinCreateModel.Author = string.Empty;
            PinCreateModel.AuthorId = string.Empty;
            PinCreateModel.AuthoriseUsersId = string.Empty;
            PinCreateModel.Private = false;
            PinCreateModel.Title = string.Empty;
            PinCreateModel.Content = string.Empty;
            PinCreateModel.CreatedTime = string.Empty;
        }

        private void PostPinButton_ClickPreJob()
        {
            DropPinProgressBar.Visibility = Visibility.Visible;
            DropPinProgressBar.IsIndeterminate = true;
            DropPinButton.IsEnabled = false;
        }

        private void PostPinButton_ClickPostJob()
        {
            DropPinProgressBar.Visibility = Visibility.Collapsed;
            DropPinProgressBar.IsIndeterminate = false;
            DropPinButton.IsEnabled = true;
            CloseMenuDownButton_Click(null, null);

            try
            {
                AddPinToMyPinsUi(PMData.PinsList.Last());

            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("PostPinButton_ClickPostJob: could not get the last pin:", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void PostPinButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in from object item in TargetLongListSelector.SelectedItems select item as PMUserModel)
            {
                if (user == null)
                {
                    Logs.Error.ShowError("PostPinButton_Click: user is null", Logs.Error.ErrorsPriority.NotCritical);
                    continue;
                }
                Logs.Output.ShowOutput(user.Pseudo);
            }

            if (_geoLocation == null)
                if (AccessLocationMsgBox() != 1)
                    return;

            if (_geoLocation == null || _geoLocation.GeopositionUser == null)
            {
                Logs.Error.ShowError("PostPinButton_Click: " + (_geoLocation == null ? "_geoLocation" : "GeopositionUser") + " is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }
    
            var pc = new PMPinController(RequestType.CreatePin, PostPinButton_ClickPostJob);

            PostPinButton_ClickPreJob();

            PinCreateModel.Title = PinCreateTitleTextBox.Text;
            PinCreateModel.Content = (PinCreateModel.PinType == PMPinModel.PinsType.Event ? FormatDateAndTimeForEvent() : "") + PinCreateMessageTextBox.Text;
            PinCreateModel.ContentType = PMPinModel.PinsContentType.Text;
            PinCreateModel.AuthorId = PMData.CurrentUserId;
            PinCreateModel.AuthoriseUsersId = FormatAuthoriseUsersId();

            pc.CreatePin(_geoLocation.GeopositionUser, PinCreateModel);
        }

        private string FormatAuthoriseUsersId()
        {
            var builder = new StringBuilder();

            for (var pos = 0; pos < TargetLongListSelector.SelectedItems.Count; pos++)
            {
                var user = (TargetLongListSelector.SelectedItems[pos]) as PMUserModel;
                
                if (user == null)
                {
                    Logs.Error.ShowError("FormatAuthoriseUsersId: user is null", Logs.Error.ErrorsPriority.NotCritical);
                    continue;
                }

                builder.Append(user.Id);
                if (pos + 1 < TargetLongListSelector.SelectedItems.Count)
                    builder.Append(",");
            }
            return builder.ToString();
        }

        private string FormatDateAndTimeForEvent()
        {
            var formattedTime = string.Empty;

            if (EventDate.Value == null)
            {
                Logs.Error.ShowError("FormatDateAndTimeForEvent: Value is null", Logs.Error.ErrorsPriority.NotCritical);
                return formattedTime;
            } 
            
            if (EventTime.Value == null)
            {
                Logs.Error.ShowError("FormatDateAndTimeForEvent: value is null", Logs.Error.ErrorsPriority.NotCritical);
                return formattedTime;
            }

            formattedTime += EventDate.Value.Value.Year;
            formattedTime += "-";
            formattedTime += EventDate.Value.Value.Month;
            formattedTime += "-";
            formattedTime += EventDate.Value.Value.Day;

            formattedTime += " ";

            formattedTime += EventTime.Value.Value.Hour;
            formattedTime += ":";
            formattedTime += EventTime.Value.Value.Minute;
            formattedTime += ":";
            formattedTime += EventTime.Value.Value.Second;
            formattedTime += ";";

            return formattedTime;
        }

        private void PinListPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pinItem = (PinListPicker.SelectedItem as PinItem);

            if (pinItem == null)
            {
                Logs.Error.ShowError("PinListPicker_OnSelectionChanged: pinItem is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            if (pinItem.PinType == PMPinModel.PinsType.PrivateMessage || pinItem.PinType == PMPinModel.PinsType.PrivateEvent || pinItem.PinType == PMPinModel.PinsType.PrivateView)
            {
                PinCreateModel.Private = true;
                PinCreateModel.PinType = pinItem.PinType - 6;

                if (PMData.UserList.Count > 0)
                    TargetLongListSelector.ItemsSource = PMData.UserList;
            }
            else
            {
                PinCreateModel.PinType = pinItem.PinType;
                PinCreateModel.Private = false;
            }

            EventStackPanel.Visibility = PinCreateModel.PinType == PMPinModel.PinsType.Event ? Visibility.Visible : Visibility.Collapsed;
            TargetStackPanel.Visibility = PinCreateModel.Private == true ? Visibility.Visible : Visibility.Collapsed;
            NoContactsTextBlock.Visibility = PMData.UserList.Count > 0 ? Visibility.Collapsed : Visibility.Visible;

            TargetLongListSelector.Height = TargetLongListSelector.ItemsSource == null ? TargetLongListSelector.Height = 0 : new GridLength(100).Value;

            CheckCanCreatePin();
        }

        private void PinCreateTitleTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PinCreateTitleTextBox.Background = new SolidColorBrush(Colors.DarkGray);
            PinCreateTitleTextBox.BorderThickness = new Thickness(0);

            if (PinCreateTitleTextBox.Text.Equals(AppResources.CreatePinDescriptionTitle) == true)
                PinCreateTitleTextBox.Text = "";
        }

        private void PinCreateTitleTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (PinCreateTitleTextBox.Text.Trim().Length == 0)
                PinCreateTitleTextBox.Text = AppResources.CreatePinDescriptionTitle;
        }

        private void PinCreateMessageTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PinCreateMessageTextBox.Background = new SolidColorBrush(Colors.DarkGray); ;
            PinCreateMessageTextBox.BorderThickness = new Thickness(0);
            
            if (PinCreateMessageTextBox.Text.Equals(AppResources.CreatePinDescription) == true)
                PinCreateMessageTextBox.Text = "";
        }

        private void PinCreateMessageTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (PinCreateMessageTextBox.Text.Trim().Length == 0)
                PinCreateMessageTextBox.Text = AppResources.CreatePinDescription;
        }

        private void PinCreateTitleTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (PinCreateTitleTextBox.Text.Equals(AppResources.CreatePinDescriptionTitle) == false && PinCreateTitleTextBox.Text.Length > 0)
                PinCreateModel.Title = PinCreateTitleTextBox.Text;
         
            CheckCanCreatePin();
        }

        private void PinCreateMessageTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (PinCreateMessageTextBox.Text.Equals(AppResources.CreatePinDescription) == false && PinCreateMessageTextBox.Text.Length > 0)
                PinCreateModel.Content = PinCreateMessageTextBox.Text;
            
            CheckCanCreatePin();
        }

        private void TargetLongListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PinCreateModel.AuthoriseUsersId = FormatAuthoriseUsersId();
            CheckCanCreatePin();
        }

        /// ////////////////////////////////////////      Commments     ////////////////////////////////////////

        private void PinCommentPostButton_OnClick(object sender, EventArgs eventArgs)
        {
            var pinController = new PMPinController(RequestType.CreatePinMessage, PinCommentPostButton_PostResponse);

            pinController.CreatePinMessage(_currentPinFocused.Id, PMPinModel.PinsContentType.Text, CommentChatBubble.Text);

            CreateAppBarMap();

            if (_isUnderMenuOpen == false)
                ApplicationBar.IsVisible = true;

            Focus();
        }

        private void PinCommentPostButton_PostResponse()
        {
            Dispatcher.BeginInvoke(() =>
            {
                CommentChatBubble.Text = "";
                AddCommentsToUi();
            });
        }

        private void CreateAppBarMap()
        {
            ApplicationBar.Buttons.Clear();

            var button1 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Menu/menu_icon_appbar.png", UriKind.Relative),
                Text = AppResources.Menu
            };
            button1.Click += OpenClose_Left;

            var button2 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Icons/flag_orange_icon_appbar.png", UriKind.Relative),
                Text = AppResources.Notifications
            };
            button2.Click += MenuDownNotification_OnClick;

            var button3 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Icons/contact_orange_icon_appbar.png", UriKind.Relative),
                Text = AppResources.Contacts
            };
            button3.Click += MenuDownContacts_OnClick;

            var button4 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Icons/logo_flat_orange_appbar.png", UriKind.Relative),
                Text = AppResources.Pins
            };
            button4.Click += Open_Right;

            ApplicationBar.Buttons.Add(button1);
            ApplicationBar.Buttons.Add(button2);
            ApplicationBar.Buttons.Add(button3);
            ApplicationBar.Buttons.Add(button4);
        }

        private void CreateAppBarComment()
        {
            ApplicationBar.Buttons.Clear();

            var button1 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Icons/check.png", UriKind.Relative),
                Text = "Valider"
            };
            button1.Click += PinCommentPostButton_OnClick;

            var button2 = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Images/Icons/cancel.png", UriKind.Relative),
                Text = "Annuler"
            };
            button2.Click += PinCommentCancelButton_OnClick;

            ApplicationBar.Buttons.Add(button1);
            ApplicationBar.Buttons.Add(button2);
        }
    
        private void PinCommentCancelButton_OnClick(object sender, EventArgs e)
        {
            CommentChatBubble.Text = "";

            CreateAppBarMap();

            ApplicationBar.IsVisible = _isUnderMenuOpen == false;
            Focus();
        }

        private void PinCommentContentTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (CommentChatBubble.Text.Trim().Length == 0)
                PinCommentTipContentTextBox.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = false;
        }

        private void PinCommentTipContentTextBox_OnTap(object sender, GestureEventArgs e)
        {
            CommentChatBubble.Focus();
        }

        private void PinCommentContentTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            CreateAppBarComment();

            LockUnlockCommentCheck(CommentChatBubble.Text.Trim().Length != 0);

            PinCommentTipContentTextBox.Visibility = Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        private void LockUnlockCommentCheck(bool enable)
        {
            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = enable;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("PinCommentContentTextBox_OnGotFocus: cannot enable appbar check button", exp,
                    Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void CommentChatBubble_OnTextInput(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            LockUnlockCommentCheck(CommentChatBubble.Text.Length != 0);
            //if (ApplicationBar.IsVisible == false)
            //    ApplicationBar.IsVisible = true;
        }

        /////////////////////////// NOTIFICATIONS ///////////////////////////////
        /// 
        /// 
        private void NotificationGrid_OnTap(object sender, GestureEventArgs e)
        {
            NotificationGrid.Visibility = Visibility.Collapsed;
            MenuDownNotification_OnClick(null, null);
        }

        public void NotificationUpdateUi()
        {
            Dispatcher.BeginInvoke(() =>
            {
                NotificationGrid.Visibility = Visibility.Visible;
                NotificationAddItem();
            });
        }

        private void NotificationAddItem()
        {
            NotificationStackPanel.Children.Add(new TextBox
            {
                Text = "dsqdfjpqjdfqjfmqs",
                FontSize = 35
            });
        }
    }
}