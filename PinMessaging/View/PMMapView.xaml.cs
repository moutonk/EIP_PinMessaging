using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
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
        private enum CurrentMapPageView { MapView, LeftMenuView, RightMenuView, UnderMenuView}

        private static CurrentMapPageView _currentView = CurrentMapPageView.MapView;
        private static bool _isUnderMenuOpen = false;
        private static readonly PMPinModel PinCreateModel = new PMPinModel();
        private PMPinModel _currentPinFocused;
     
        readonly MapLayer _mapLayer = new MapLayer();
        readonly MapOverlay _userSpotLayer = new MapOverlay();
        readonly UserLocationMarker _userSpot = new UserLocationMarker();
        public PMGeoLocation _geoLocation = null;

        public PMMapView()
        {
            InitializeComponent();

            //central page
            ImgTarget.ImageSource = new BitmapImage(Paths.TargetButton);
     /*      ImgMenuButton.ImageSource = new BitmapImage(Paths.MenuButton);
            ImgNotificationButton.ImageSource = new BitmapImage(Paths.NotificationsButton);
            ImgContactsButton.ImageSource = new BitmapImage(Paths.ContactsButton);
            ImgPinsButton.ImageSource = new BitmapImage(Paths.PinsButton);*/

            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Text = AppResources.Menu;
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).Text = AppResources.Notifications;
                (ApplicationBar.Buttons[2] as ApplicationBarIconButton).Text = AppResources.Contacts;
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).Text = AppResources.Pins;

                (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Text = AppResources.RefreshPins;
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
      
            bool? accessLoc =  RememberConnection.GetAccessLocation();
            Logs.Output.ShowOutput(accessLoc.ToString());

            if (accessLoc == true)
            {
                Logs.Output.ShowOutput("accesloc true");
                LaunchLocalization();      
            }

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

        private void AdaptUiUnderMenuClick(RowDefinition exept, bool pinDescFull, bool contactFull)
        {
            for (var i = 1; i < UnderMenuGrid.RowDefinitions.Count; i++)
            {
                UnderMenuGrid.RowDefinitions[i].Height = UnderMenuGrid.RowDefinitions[i].Equals(exept) == false ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            }
            UnderMenuPinDescriptionGrid.Height = (pinDescFull == true ? UnderMenuPinDescriptionScrollView.Height : 0);
            UnderMenuContactPanel.Height = (contactFull == true ? UnderMenuContactScrollViewer.Height : 0);
        }

        private void MenuDown_PinOnClick()
        {
            Dispatcher.BeginInvoke(() =>
            {
                _currentView = CurrentMapPageView.UnderMenuView;
                _isUnderMenuOpen = true;
                _enableSwipe = false;
                ApplicationBar.IsVisible = false;

                DownMenuTitle.Text = AppResources.Pin;
                AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[3], true, false);
      
                MainGridMap.RowDefinitions[2].Height = new GridLength(0);
                MoveAnimationUp.Begin();
            });
        }

        private void MenuDown_NotificationOnClick()
        {
            DownMenuTitle.Text = AppResources.Notifications;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[2], false, false);
        }

        private void MenuDown_ContactOnClick()
        {
            DownMenuTitle.Text = AppResources.Contacts;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[1], false, true);
        }

        private void MenuDown_CreatePin()
        {
            DownMenuTitle.Text = AppResources.CreatePinTitle;
            AdaptUiUnderMenuClick(UnderMenuGrid.RowDefinitions[4], false, false);

            VisibilityExpandView.IsExpanded = true;
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
            var pinController = new PMPinController(RequestType.GetPins, null);

            pinController.GetPins(Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Latitude.ToString()),
                                  Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Longitude.ToString()));
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
            if (sender.Equals(ButtonPins))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_PinOnClick();
                MenuDown_CommonActionsAfter();
            }
           /* else if (sender.Equals(NotificationButton) || sender.Equals(NotificationButtonTextBlock))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_NotificationOnClick();
                MenuDown_CommonActionsAfter();
            }
            else if (sender.Equals(ContactsButton) || sender.Equals(ContactsButtonTextBlock))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_ContactOnClick();
                MenuDown_CommonActionsAfter();
            }*/
            else if (sender.Equals(CreatePinButton))
            {
                MenuDown_CommonActionsBefore();
                MenuDown_CreatePin();
                MenuDown_CommonActionsAfter();
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
            int retValue = Utils.Utils.CustomMessageBox(new[] {"Yes", "No"}, AppResources.QuitAppTitle, AppResources.QuitApp);

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

        private void RefreshPinButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pinController = new PMPinController(RequestType.GetPins, null);

            pinController.GetPins(Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Latitude.ToString()), 
                                  Utils.Utils.ConvertDoubleCommaToPoint(_geoLocation.GeopositionUser.Coordinate.Longitude.ToString()));
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

        void MoveViewWindow(double left)
        {
            _viewMoved = true;

            if (left == 0 || left == -840)
            {
                ApplicationBar.Mode = ApplicationBarMode.Minimized;
                Map.IsEnabled = false;
            }
            else
            {
                ApplicationBar.Mode = ApplicationBarMode.Default;
                Map.IsEnabled = true;
            }

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
            ContactNameOnTapSub(PMData.UserId);
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

        /////////////////////////////////////////////////   RIGHT MENU ///////////////////////////////////////////////
        
        private void MyPinsPivotItem_OnSelected()
        {
            var myPinsCollection = PMData.PinsList.Where(pin => pin.AuthorId == PMData.UserId);

            foreach (var pinModel in myPinsCollection)
            {
                var item = new TextBox() { Text = pinModel.Content, Width = 300, Tag = pinModel};
                item.Tap += MyPinOnDelete;
                MyPinsGrid.Children.Add(item);
            }
        }

        private void MyPinOnDelete_Post()
        {
            Logs.Output.ShowOutput("pins deleted UI");   
        }

        private void MyPinOnDelete(object sender, GestureEventArgs gestureEventArgs)
        {
            //var pc = new PMPinController(RequestType.DeletePin, MyPinOnDelete_Post);

            //pc.DeletePin(((sender as TextBox).Tag as PMPinModel).Id);
        }

        private void PivotPins_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotPins.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    break;

                case 2:
                    MyPinsGrid.Children.Clear();
                    MyPinsPivotItem_OnSelected();
                    break;
            }
        }

        ////////////////////////////////////////////////    Down Menu    /////////////////////////////////////////////

        private void CloseMenuDownButton_Click(object sender, RoutedEventArgs e)
        {
            Map_OnTouch(sender, e);
        }

        /// ///////////////////////////////////////////    Contact    //////////////////////////////////////////////////

        private Grid CreateContactItem()
        {
            var contactGrid = new Grid {Margin = new Thickness(-5,0,0,0)};

            contactGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(110) });
            //     contactGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(70) });

            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(170) });
            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });

            var contactImg = new Image() { Source = new BitmapImage(new Uri("/Images/8.jpg", UriKind.Relative)) };
            var contactName = new TextBlock() { Text = PMData.User.Login, FontSize = 25, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 0, 0)};
            var onlineImg = new Image() { Source = new BitmapImage(Paths.TargetButton), Height = 50, Width = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 0)};
            /*var privateMsg = new TextBlock() { Text = "Private message", FontSize = 20, Margin = new Thickness(10, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
            var userProfil = new TextBlock() { Text = "User profil", FontSize = 20, Margin = new Thickness(10, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
            var localise = new TextBlock() { Text = "Localize", FontSize = 20, Margin = new Thickness(10, 0, 10, 0), VerticalAlignment = VerticalAlignment.Center };
*/
            contactGrid.Children.Add(contactImg);
            contactGrid.Children.Add(contactName);
            contactGrid.Children.Add(onlineImg);
           /* contactGrid.Children.Add(privateMsg);
            contactGrid.Children.Add(userProfil);
            contactGrid.Children.Add(localise*/

            Grid.SetRow(contactImg, 0);
            Grid.SetColumn(contactImg, 0);

            Grid.SetRow(contactName, 0);
            Grid.SetColumn(contactName, 1);

            Grid.SetRow(onlineImg, 0);
            Grid.SetColumn(onlineImg, 2);

/*            Grid.SetRow(privateMsg, 1);
            Grid.SetColumn(privateMsg, 0);

            Grid.SetRow(userProfil, 1);
            Grid.SetColumn(userProfil, 1);

            Grid.SetRow(localise, 1);
            Grid.SetColumn(localise, 2);*/

            return contactGrid;
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
                Logs.Output.ShowOutput("ContactNameOnTap: could not get the info about the author");
            }
        }

        private void ContactNameOnTap(object sender, GestureEventArgs gestureEventArgs)
        {
            var img = sender as Image;

            try
            {
                if (img == null)
                {
                    var tb = sender as TextBlock;

                    if (tb != null)
                        ContactNameOnTapSub((tb.Tag as PMUserModel).Id);
                }
                else
                {
                    ContactNameOnTapSub((img.Tag as PMUserModel).Id);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ContactNameOnTap: Tag is empty", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void AddContactUI(PMUserModel user)
        {
            var contactGrid = new Grid { Margin = new Thickness(-5, 0, 0, 0) };

            contactGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(110) });

            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(170) });
            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contactGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });

            var contactImg = new Image() { Source = new BitmapImage(new Uri("/Images/8.jpg", UriKind.Relative)), Tag = user };
            var contactName = new TextBlock() { Text = user.Login, Tag = user, FontSize = 25, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 0, 0) };
            var onlineImg = new Image() { Source = new BitmapImage(Paths.TargetButton), Height = 50, Width = 50, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 10, 0) };

            contactImg.Tap += ContactNameOnTap;
            contactName.Tap += ContactNameOnTap;

            contactGrid.Children.Add(contactImg);
            contactGrid.Children.Add(contactName);
            contactGrid.Children.Add(onlineImg);

            Grid.SetRow(contactImg, 0);
            Grid.SetColumn(contactImg, 0);

            Grid.SetRow(contactName, 0);
            Grid.SetColumn(contactName, 1);

            Grid.SetRow(onlineImg, 0);
            Grid.SetColumn(onlineImg, 2);

            UnderMenuContactPanel.Children.Add(contactGrid);
        }

        private static void AddContactCode(PMUserModel user)
        {
            PMData.AddToQueueUserList(user);
        }

        public void AddContact(PMUserModel user)
        {
            if (PMMapContactController.IsFavoriteUnique(user) == true)
            {
                AddContactUI(user);
                AddContactCode(user);
            }
        }

        private void RemoveContactUI(PMUserModel user)
        {
            if (UnderMenuContactPanel.Children.Count == 0)
                return;

            try
            {
                var rep = UnderMenuContactPanel.Children.First(contact => ((((contact as Grid).Children[0]) as Image).Tag as PMUserModel).Id == user.Id);

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
            RemoveContactUI(user);
            RemoveContactCode(user);
        }

        /// //////////////////////////////////////////     Pin description      ////////////////////////////////////////

        private void AddCommentsToUi()
        {
            foreach (var tb in PMData.PinsCommentsListTmp.Select(comment => new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 25, Margin = new Thickness(30, 0, 30, 0), Text = comment.Content }))
            {
                CommentStackPanel.Children.Add(tb);
            }
            PMData.AddToQueuePinComments(PMData.PinsCommentsListTmp);
            PMData.PinsCommentsListTmp.Clear();
        }

        public void PinTapped(PMPinModel pin)
        {
            var pinC = new PMPinController(RequestType.GetPinMessages, GetPinMessages_Post);

            pinC.GetPinMessage(pin);

            PinTitleDescriptionTextBlock.Text = pin.Title;
            PinMessageDescriptionTextBlock.Text = pin.Content + (pin.PinType == PMPinModel.PinsType.Event ? Environment.NewLine +
                                                                                                            "Time of the event: " +
                                                                                                            (pin.DateTime != null ? pin.DateTime.Aggregate("", (current, keyValuePair) => current + (keyValuePair + " ")) : "null") : "");
            PinAuthorDescriptionTextBlock.Text = pin.Author;
            PinAuthorDescriptionTextBlock.Tag = pin;
            PinDescriptionImage.Source = Paths.PinsMapImg[pin.PinType];

            var pic = PMData.GetUserProfilPicture(pin.AuthorId);

            if (pic != null)
                AuthorPicture.Source = pic.Img;
            else
                AuthorPicture.Source = new BitmapImage(Paths.NeutralProfilPicture);

            if (pin.Url != null)
            {
                UnderMenuPinDescriptionGrid.RowDefinitions[3].Height = new GridLength(300);
                PinImage.Source = PMWebService.DownloadImageUrl(pin.Url);
            }
            else
            {
                UnderMenuPinDescriptionGrid.RowDefinitions[3].Height = new GridLength(0);
            }

            _currentPinFocused = pin;

            MenuDown_OnClick(ButtonPins, new RoutedEventArgs());
        }

        public void GetPinMessages_Post()
        {
            CommentStackPanel.Children.Clear();
            AddCommentsToUi();
        }

        private void PinAuthorDescriptionTextBlock_PostTap()
        {
            try
            {
                if (PMData.User != null)
                    NavigationService.Navigate(PMData.User.Id != PMData.UserId ? Paths.UserProfilView : Paths.CurrentUserProfilView);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.Critical);
            }
        }

        private void PinAuthorDescriptionTextBlock_OnTap(object sender, GestureEventArgs e)
        {
            var pin = PinAuthorDescriptionTextBlock.Tag as PMPinModel;

            if (pin != null)
            {
                var userController = new PMUserController(RequestType.User, PinAuthorDescriptionTextBlock_PostTap);

                userController.GetUserInfos(pin.AuthorId);
            }
            else
            {
                Logs.Output.ShowOutput("PinAuthorDescriptionTextBlock_OnTap: could not get the info about the author");
            }
        }

        ////////////////////////////////////////////////    Create pin         //////////////////////////////////////////

        private void CreatePinImgOnTap(object sender, GestureEventArgs gestureEventArgs)
        {
            var img = sender as Image;

            if (img == null)
            {
                Logs.Output.ShowOutput("NULL");
                return;
            }
            Logs.Output.ShowOutput(img.Name);

            if (img.Name.Equals("PublicMsg"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.PublicMessage;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PublicEvent"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.Event;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Visible;
            }
            else if (img.Name.Equals("PublicView"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.View;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PublicCourseStart"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseStart;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PublicCourseNext"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseNextStep;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PublicCourseLast"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseLastStep;
                PinCreateModel.Private = false;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PrivateMsg"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.PublicMessage;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;
            }
            else if (img.Name.Equals("PrivateEvent"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.Event;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Visible;
            }
            else if (img.Name.Equals("PrivateView"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.View;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PrivateCourseStart"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseStart;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PrivateCourseNext"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseNextStep;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;

            }
            else if (img.Name.Equals("PrivateCourseLast"))
            {
                PinCreateModel.PinType = PMPinModel.PinsType.CourseLastStep;
                PinCreateModel.Private = true;
                ExpanderViewEventDate.Visibility = Visibility.Collapsed;
            }

            CloseAllExpanderExcept(TitleExpandView);
            CheckCanCreatePin();
        }

        private void CloseAllExpanderExcept(ExpanderView exp)
        {
            var expTab = new ExpanderView[6]
            {
                VisibilityExpandView,
                PinTypeExpandView,
                TitleExpandView,
                DescriptionExpandView,
                ExpanderViewEventDate,
                TargetExpanderViewExpandView
            };

            foreach (var elem in expTab)
            {
                elem.IsExpanded = (elem == exp);
            }
        }

        private void LoadCreatePinsPublicPins()
        {
            var img1 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PublicMessage], Name = "PublicMsg" }; img1.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img1);

            var img2 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.Event], Name = "PublicEvent" }; img2.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img2);

            var img3 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.View], Name = "PublicView" }; img3.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img3);

            var img4 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.CourseStart], Name = "PublicCourseStart" }; img4.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img4);

            var img5 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.CourseNextStep], Name = "PublicCourseNext" }; img5.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img5);

            var img6 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.CourseLastStep], Name = "PublicCourseLast" }; img6.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img6);
        }

        private void LoadCreatePinsPrivatePins()
        {
            var img1 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateMessage], Name = "PrivateMsg" }; img1.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img1);

            var img2 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateEvent], Name = "PrivateEvent" }; img2.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img2);

            var img3 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateView], Name = "PrivateView" }; img3.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img3);

            var img4 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateCourseStart], Name = "PrivateCourseStart" }; img4.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img4);

            var img5 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateCourseNextStep], Name = "PrivateCourseNext" }; img5.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img5);

            var img6 = new Image { Source = Paths.PinsMapImg[PMPinModel.PinsType.PrivateCourseLastStep], Name = "PrivateCourseLast" }; img6.Tap += CreatePinImgOnTap;
            PinTypeScollStackPanel.Children.Add(img6);
        }


        private void TargetExpanderViewExpandView_OnExpandedExpandView_OnExpanded(object sender, RoutedEventArgs e)
        {

        }

        private void CreateTargetLongList()
        {
            TargetLongListSelector.ItemsSource = PMData.UserList;
        }

        private void PinTypeExpandView_OnExpanded(object sender, RoutedEventArgs e)
        {
            if (PinTypeScollStackPanel.Children.Count == 0)
            {
                LoadCreatePinsPublicPins();
                LoadCreatePinsPrivatePins();
            }
            CreateTargetLongList();
        }

        private void TargetExpanderAdaptView(bool priv)
        {
         if (priv == false)
                TargetExpanderViewExpandView.Visibility = Visibility.Collapsed;
            else
                TargetExpanderViewExpandView.Visibility = Visibility.Visible;   
        }

        private void VisibilityExpandViewPublicGrid_OnTap(object sender, GestureEventArgs e)
        {
            PinTypeScollStackPanel.Children.Clear();
            LoadCreatePinsPublicPins();
            CloseAllExpanderExcept(PinTypeExpandView);

            TargetExpanderAdaptView(false);
        }

        private void VisibilityExpandViewPrivateGrid_OnTap(object sender, GestureEventArgs e)
        {
            PinTypeScollStackPanel.Children.Clear();
            LoadCreatePinsPrivatePins();
            CloseAllExpanderExcept(PinTypeExpandView);

            TargetExpanderAdaptView(true);
        }

        private void CheckCanCreatePin()
        {
            bool canCreate = false;

            if (PinCreateModel.Content.Length != 0 && PinCreateModel.Title.Length != 0)
            {
                if (PinCreateModel.PinType != PMPinModel.PinsType.Default)
                {
                    if (PinCreateModel.PinType == PMPinModel.PinsType.Event)
                    {
                        if (PinCreateModel.CreationTime.Length != 0)
                        {
                            canCreate = true;
                        }
                    }
                    else
                    {
                        canCreate = true;
                    }
                }
            }

            //if (canCreate == true)
            DropPinButton.Visibility = Visibility.Visible;
        }

        private void TitleExpandView_OnExpanded(object sender, RoutedEventArgs e)
        {
        }

        private void DescriptionExpandView_OnExpanded(object sender, RoutedEventArgs e)
        {
            TitleExpandView.IsExpanded = false;
        }

        private void TitleExpandViewTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PinCreateModel.Title = TitleExpandViewTextBox.Text;
            CheckCanCreatePin();
        }

        private void DescriptionExpandViewTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PinCreateModel.Content = DescriptionExpandViewTextBox.Text;
            CheckCanCreatePin();
        }

        private void ResetCreatePinModel()
        {
            PinCreateModel.Id = string.Empty;
            PinCreateModel.Lang = string.Empty;
            PinCreateModel.PinType = PMPinModel.PinsType.Default;
            PinCreateModel.Author = string.Empty;
            PinCreateModel.AuthorId = string.Empty;
            PinCreateModel.AuthoriseUsersId = null;
            PinCreateModel.Private = false;
            PinCreateModel.Title = string.Empty;
            PinCreateModel.Content = string.Empty;
            PinCreateModel.CreationTime = string.Empty;
        }

        private void PostPinButton_ClickPreJob()
        {
            DropPinProgressBar.Visibility = Visibility.Visible;
            DropPinProgressBar.IsIndeterminate = true;
            DropPinButton.IsEnabled = false;
            //NewPinPivotItem.IsEnabled = false;
        }

        private void PostPinButton_ClickPostJob()
        {
            DropPinProgressBar.Visibility = Visibility.Collapsed;
            DropPinProgressBar.IsIndeterminate = false;
            DropPinButton.IsEnabled = true;
            CloseMenuDownButton_Click(null, null);
            //NewPinPivotItem.IsEnabled = true;
        }

        private void PostPinButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in TargetLongListSelector.SelectedItems)
            {
                Logs.Output.ShowOutput((item as PMUserModel).Login);
            }

            if (_geoLocation == null)
                if (AccessLocationMsgBox() != 1)
                    return;
            if (_geoLocation != null && _geoLocation.GeopositionUser == null)
                return;
           
            var pc = new PMPinController(RequestType.CreatePin, PostPinButton_ClickPostJob);

            PostPinButton_ClickPreJob();

            PinCreateModel.Title = TitleExpandViewTextBox.Text;
            PinCreateModel.Content = (PinCreateModel.PinType == PMPinModel.PinsType.Event ? FormatDateAndTimeForEvent() : "") + DescriptionExpandViewTextBox.Text;
            //PinCreateModel.PinType = PMPinModel.PinsType.PublicMessage;
            PinCreateModel.ContentType = PMPinModel.PinsContentType.Text;
         //   PinCreateModel.Private = "0";
            PinCreateModel.AuthoriseUsersId = FormatAuthoriseUsersId();

            pc.CreatePin(_geoLocation.GeopositionUser, PinCreateModel);
        }

        private string FormatAuthoriseUsersId()
        {
            var builder = new StringBuilder();

            for (var pos = 0; pos < TargetLongListSelector.SelectedItems.Count; pos++)
            {
                builder.Append(((TargetLongListSelector.SelectedItems[pos]) as PMUserModel).Id);
                if (pos + 1 < TargetLongListSelector.SelectedItems.Count)
                    builder.Append(",");
            }
            return builder.ToString();
        }

        private string FormatDateAndTimeForEvent()
        {
            var formattedTime = string.Empty;

            if (CreatePinDatePicker.Value.HasValue != true || CreatePinTimePicker.Value.HasValue != true)
                return formattedTime;

            formattedTime += CreatePinDatePicker.Value.Value.Year;
            formattedTime += "-";
            formattedTime += CreatePinDatePicker.Value.Value.Month;
            formattedTime += "-";
            formattedTime += CreatePinDatePicker.Value.Value.Day;

            formattedTime += " ";

            formattedTime += CreatePinTimePicker.Value.Value.Hour;
            formattedTime += ":";
            formattedTime += CreatePinTimePicker.Value.Value.Minute;
            formattedTime += ":";
            formattedTime += CreatePinTimePicker.Value.Value.Second;
            formattedTime += ";";

            return formattedTime;
        }

        /// ////////////////////////////////////////      Commments     ////////////////////////////////////////

        private void PinCommentPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pinController = new PMPinController(RequestType.CreatePinMessage, PinCommentPostButton_PostResponse);

            pinController.CreatePinMessage(_currentPinFocused.Id, PMPinModel.PinsContentType.Text, PinCommentContentTextBox.Text);
        }

        private void PinCommentPostButton_PostResponse()
        {
            PinCommentContentTextBox.Text = "";
            AddCommentsToUi();
        }
    }
}