﻿using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMUserProfil : PhoneApplicationPage
    {
        private readonly PMUserModel _user = null;

        public PMUserProfil()
        {
            InitializeComponent();

            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Text = AppResources.AddContacts;
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).Text = AppResources.Localize;
                (ApplicationBar.Buttons[2] as ApplicationBarIconButton).Text = AppResources.PrivateMessage;
                (ApplicationBar.Buttons[3] as ApplicationBarIconButton).Text = AppResources.SharePin;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("Loading ApplicationBar text error", exp, Logs.Error.ErrorsPriority.NotCritical);
            }

            if (PMData.User == null)
            {
                Logs.Error.ShowError("PMUserProfil: user is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            _user = PMData.User.Clone();

            LoginTextBlock.Text = _user.Pseudo;
            PointsTextBlock.Text = _user.Points;
            PinsCreatedTextBlock.Text = _user.NbrPin;
            CommentsTextBlock.Text = _user.NbrMessage;
            GradeTextBlock.Text = Utils.Utils.GetGradeInfo(_user.Grade.Type).Item1;

            //if the user is already in the contact list
            if (PMData.UserList.Any(user => user.Id == _user.Id) == true)
            {
                RemoveFavoriteUi();
            }

            var profilPic = PMData.GetUserProfilPicture(_user.Id);

            if (profilPic != null)
                ProfilPictureImage.Source = profilPic.Img;

            var userHistory = new PMHistoryController(RequestType.UserHistory, UpdateHistoryUi);

            userHistory.GetUserHistory(_user.Id);
        }

        /*HISTORY*/

        private static BitmapImage DefaultHistoryTypeImg()
        {
            return new BitmapImage(new Uri("/Images/Icons/about_white_icon@2x.png", UriKind.Relative));
        }

        private static string DefaultHistoryTypeString()
        {
            return "Unknown";
        }

        private static string GetHistoryTypeString(PMHistoryModel.HistoryType? type)
        {
            if (type == null)
                return DefaultHistoryTypeString();

            Logs.Output.ShowOutput(type.ToString());

            switch (type)
            {
                case PMHistoryModel.HistoryType.CreatePin:
                    return AppResources.HistoryCreatePin;

                case PMHistoryModel.HistoryType.ChangePin:
                    return AppResources.HistoryChangePin;

                case PMHistoryModel.HistoryType.DeletePin:
                    return AppResources.HistoryDeletePin;

                case PMHistoryModel.HistoryType.AddFavoriteUser:
                    return AppResources.HistoryAddFavoriteUser;

                case PMHistoryModel.HistoryType.AddFavoriteLocation:
                    return AppResources.HistoryAddFavoriteLocation;

                case PMHistoryModel.HistoryType.CreatePinMessage:
                    return AppResources.HistoryCommentPin;

                case PMHistoryModel.HistoryType.NewUser:
                    return AppResources.HistoryNewUser;

                default:
                    return DefaultHistoryTypeString();
            }
        }

        private static BitmapImage GetHistoryTypeImg(PMHistoryModel.HistoryType? type)
        {
            if (type == null)
                return DefaultHistoryTypeImg();

            Logs.Output.ShowOutput(type.ToString());
            
            switch (type)
            {
                case PMHistoryModel.HistoryType.CreatePin:
                    return new BitmapImage(new Uri("/Images/Logos/3_new_logo_white.png", UriKind.Relative));

                case PMHistoryModel.HistoryType.ChangePin:
                    return new BitmapImage(new Uri("/Images/Icons/edit.png", UriKind.Relative));

                case PMHistoryModel.HistoryType.DeletePin:
                    return new BitmapImage(new Uri("/Images/Icons/cross_white_icon@2x.png", UriKind.Relative));
                
                case PMHistoryModel.HistoryType.AddFavoriteUser:
                    return new BitmapImage(new Uri("/Images/Icons/add.png", UriKind.Relative));
             
                case PMHistoryModel.HistoryType.CreatePinMessage:
                    return new BitmapImage(new Uri("/Images/Icons/bubble_white_icon@2x.png", UriKind.Relative));

                default:
                   return DefaultHistoryTypeImg();
            }
        }

        private string AdaptContentForMsgPrivacy(string content)
        {
            return content.Substring(0, content.Length > 10 ? 10 : content.Length) + "...";
        }

        private void CreateHistoryItemUi(PMHistoryModel item)
        {
            var historyImage = new Image { Height = 70, Width = 70, Source = GetHistoryTypeImg(item.historyType) };
            var messageTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 25, TextAlignment = TextAlignment.Left, Text = GetHistoryTypeString(item.historyType), Height = 35 };
            var dateTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 16, TextAlignment = TextAlignment.Left, Height = 27 };
            var goToMapTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 13, VerticalAlignment = VerticalAlignment.Center, Text = AppResources.SeeOnMap, Foreground = (Brush)Application.Current.Resources["PMOrange"], Height = 25 };

            var res = Utils.Utils.ConvertStringToDouble(item.Date);

            if (res != null)
            {
                var d = Utils.Utils.ConvertFromUnixTimestamp(res);
                dateTextBlock.Text = d.ToShortDateString() + " " + d.ToShortTimeString();
            }

            var itemMyPin = new Button
            {
                Margin = new Thickness(-20, 0, 0, 0),
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Tag = item
            };
            itemMyPin.Click += ItemMyPinOnClick;

            var itemMainGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Left };
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            itemMainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70) });

            itemMyPin.Content = itemMainGrid;

            HistoryItemsStackPanel.Children.Add(itemMyPin);

            itemMainGrid.Children.Add(historyImage);

            var pinContentStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
            pinContentStackPanel.Children.Add(messageTextBlock);
            pinContentStackPanel.Children.Add(dateTextBlock);
            pinContentStackPanel.Children.Add(goToMapTextBlock);
            
            itemMainGrid.Children.Add(pinContentStackPanel);

            Grid.SetRow(historyImage, 0);
            Grid.SetColumn(historyImage, 0);

            Grid.SetRow(pinContentStackPanel, 0);
            Grid.SetColumn(pinContentStackPanel, 1);
        }

        private void ItemMyPinOnClick(object sender, RoutedEventArgs e)
        {
            var butt = sender as Button;

            if (butt == null)
            {
                Logs.Error.ShowError("ItemMyPinOnClick: butt is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var historyPin = butt.Tag as PMHistoryModel;

            if (historyPin == null)
            {
                Logs.Error.ShowError("ItemMyPinOnClick: historyPin is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            if (historyPin.Latitude == null || historyPin.Longitude == null)
            {
                Logs.Error.ShowError("ItemMyPinOnClick: historyPin.Latitude or historyPin.Longitude is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            Logs.Output.ShowOutput("Center on: " + (double)historyPin.Latitude + "  " + (double)historyPin.Longitude);
            PMMapContactController.MapCenterOn(new GeoCoordinate((double)historyPin.Latitude, (double)historyPin.Longitude));

            if (NavigationService.CanGoBack == true)
                NavigationService.GoBack();

            PMMapContactController.CloseDownMenu();
        }

        private void UpdateHistoryUi()
        {
            foreach (var historyItem in PMData.UserHistoryList)
            {
                CreateHistoryItemUi(historyItem);
            }
        }

        /*CONTACTS*/

        private void RemoveFavoriteUi()
        {
            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IconUri = new Uri("/Images/Icons/minus.png", UriKind.Relative);
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Text = AppResources.Unfriend;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("RemoveFavoriteUi: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
         }

        private void AddFavoriteUI()
        {
            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IconUri = new Uri("/Images/Icons/add.png", UriKind.Relative);
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).Text = AppResources.AddContacts;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("AddFavoriteUI: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
          }

        private void AddAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteAddedSuccess == true || PMData.WasFavoriteAddedSuccess == false)
            {
                PMMapContactController.AddNewFavoris(_user);
                RemoveFavoriteUi();                
            }
            else
            {
                /*foreach (var item in PMData.UserList)
                {
                    Logs.Output.ShowOutput(item.Pseudo);
                }
                try
                {
                    PMData.UserList.RemoveAt(PMData.UserList.FindIndex(elem => elem.Id == _user.Id));
                }
                catch (Exception exp)
                {
                    Logs.Error.ShowError("AddAsFavoriteButton_Post: not important", exp, Logs.Error.ErrorsPriority.NotCritical);
                }
                RemoveFavoriteButton_OnClick(null, null);
                PMMapContactController.RemoveFavoris(_user);*/
            }
            AddRemoveFavoriteButtonLock(false);

        }

        private void RemoveAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteRemovedSuccess == true || PMData.WasFavoriteRemovedSuccess == false)
            {
                PMMapContactController.RemoveFavoris(_user);
                AddFavoriteUI();
            }
            /*else
            {
                PMData.UserList.RemoveAt(PMData.UserList.FindIndex(elem => elem.Id == _user.Id));
                PMMapContactController.RemoveFavoris(_user);
                AddFavoriteUI();
            }*/
            AddRemoveFavoriteButtonLock(false);

        }

        private void AddRemoveFavoriteButtonLock(bool lockStatus)
        {
            UserProfilProgressBar.IsIndeterminate = lockStatus;
            UserProfilProgressBar.Visibility = (lockStatus ? Visibility.Visible : Visibility.Collapsed);

            try
            {
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = !lockStatus;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("AddRemoveFavoriteButtonLock: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void AddRemoveFavoriteButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_user != null)
            {
                AddRemoveFavoriteButtonLock(true);

                //if the user is already in the contact list
                if (PMData.UserList.Any(user => user.Id == _user.Id) == true)
                {
                    RemoveFavoriteButton_OnClick(sender, eventArgs);
                }
                else
                {
                    //RemoveFavoriteButton_OnClick(sender, eventArgs);
                    AddFavoriteButton_OnClick(sender, eventArgs);
                }      
            }
            else
            {
                Logs.Error.ShowError("AddRemoveFavoriteButton_OnClick: User is null", Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void AddFavoriteButton_OnClick(object sender, EventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.AddFavoriteUser, AddAsFavoriteButton_Post);

            PMData.WasFavoriteAddedSuccess = false;
            favController.AddFavoriteUser(_user.Id);
        }

        private void RemoveFavoriteButton_OnClick(object sender, EventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.RemoveFavoriteUser, RemoveAsFavoriteButton_Post);

            PMData.WasFavoriteRemovedSuccess = false;
            favController.RemoveFavoriteUser(_user.Id);
        }

        private void PrivateMsgButton_OnClick(object sender, EventArgs eventArgs)
        {
            PMMapPinController.DropPrivatePin(_user);

            try
            {
                if (NavigationService.CanGoBack == true)
                    NavigationService.GoBack();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("PrivateMsgButton_OnClick: cannot go back in the map page", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }   
}