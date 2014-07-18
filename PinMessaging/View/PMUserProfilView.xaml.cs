using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
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

            if (PMData.User != null)
            {
                _user = PMData.User.Clone();

                UserNameTextBlock.Text = _user.Login;

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
        }

        /*HISTORY*/

        private static BitmapImage DefaultHistoryTypeImg()
        {
            return new BitmapImage(new Uri("/Images/Pins/event_icon.png", UriKind.Relative));
        }

        private static BitmapImage GetHistoryTypeImg(PMHistoryModel.HistoryType? type)
        {
            if (type == null)
                return DefaultHistoryTypeImg();

            switch (type)
            {
                case PMHistoryModel.HistoryType.CreatePin:
                    return new BitmapImage(new Uri("/Images/Pins/message_icon.png", UriKind.Relative));

                case PMHistoryModel.HistoryType.CreatePinMessage:
                    return new BitmapImage(new Uri("/Images/Icons/bubble_white_icon.png", UriKind.Relative));

                default:
                   return DefaultHistoryTypeImg();
            }
        }

        private void CreateHistoryItemUi(PMHistoryModel item)
        {
            var historyImage = new Image { Height = 50, Width = 50, Source = GetHistoryTypeImg(item.historyType) };
            var messageTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, Text = item.Content };
            var messageDateTextBlock = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 12, TextAlignment = TextAlignment.Right, Text = "05/21/14 at 5:01 PM" };

            var itemMainGrid = new Grid();
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            itemMainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            itemMainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });

            HistoryItemsStackPanel.Children.Add(itemMainGrid);

            itemMainGrid.Children.Add(historyImage);

            var pinContentStackPanel = new StackPanel { Margin = new Thickness(20, 0, 0, 0) };
            pinContentStackPanel.Children.Add(messageTextBlock);
            pinContentStackPanel.Children.Add(messageDateTextBlock);

            itemMainGrid.Children.Add(pinContentStackPanel);

            Grid.SetRow(historyImage, 0);
            Grid.SetColumn(historyImage, 0);

            Grid.SetRow(pinContentStackPanel, 0);
            Grid.SetColumn(pinContentStackPanel, 1);

            var line = new Canvas { Background = (Brush)Resources["PhoneProgressBarBackgroundBrush"], Height = 5 };

            HistoryItemsStackPanel.Children.Add(line);
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
            ContactImage.Source = new BitmapImage(new Uri("/Images/Icons/flag_orange_icon@2x.png", UriKind.Relative));
            ContactTextBlock.Text = AppResources.Unfriend;
        }

        private void AddFavoriteUI()
        {
            ContactImage.Source = new BitmapImage(new Uri("/Images/Icons/contact_orange_icon.png", UriKind.Relative));
            ContactTextBlock.Text = AppResources.AddContacts;
        }

        private void AddAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteAddedSuccess == true)
            {
                PMMapContactController.AddNewFavoris(_user);
                RemoveFavoriteUi();                
            }
            AddRemoveFavoriteButtonLock(false);

        }

        private void RemoveAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteRemovedSuccess == true)
            {
                PMMapContactController.RemoveFavoris(_user);
                AddFavoriteUI();
            }
            AddRemoveFavoriteButtonLock(false);

        }

        private void AddRemoveFavoriteButtonLock(bool lockStatus)
        {
            UserProfilProgressBar.IsIndeterminate = lockStatus;
            UserProfilProgressBar.Visibility = (lockStatus ? Visibility.Visible : Visibility.Collapsed);
            ContactButton.IsEnabled = !lockStatus;
        }

        private void AddRemoveFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                AddRemoveFavoriteButtonLock(true);

                //if the user is already in the contact list
                if (PMData.UserList.Any(user => user.Id == _user.Id) == true)
                {
                    RemoveFavoriteButton_OnClick(sender, e);
                }
                else
                {
                    AddFavoriteButton_OnClick(sender, e);
                }      
            }
            else
            {
                Logs.Error.ShowError("User is null", Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void AddFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.AddFavoriteUser, AddAsFavoriteButton_Post);

            PMData.WasFavoriteAddedSuccess = false;
            favController.AddFavoriteUser(_user.Id);
        }

        private void RemoveFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.RemoveFavoriteUser, RemoveAsFavoriteButton_Post);

            PMData.WasFavoriteRemovedSuccess = false;
            favController.RemoveFavoriteUser(_user.Id);
        }

        private void PrivateMsgButton_OnClick(object sender, RoutedEventArgs e)
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

        private void CreateHistoryEvents()
        {
            
        }
    }   
}