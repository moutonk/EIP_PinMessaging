using System;
using System.Linq;
using System.Windows;
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
                    RemoveFavoriteUI();
                }

                var profilPic = PMData.GetUserProfilPicture(_user.Id);

                if (profilPic != null)
                    ProfilPictureImage.Source = profilPic.Img;

                /* PointsTextBlock.Text = _user.Points;
                NbrMsgTextBlock.Text = _user.NbrMessage;
                NbrPinTextBlock.Text = _user.NbrPin;
                LoginTextBlock.Text = _user.Login;
                GradeTextBlock.Text = _user.Grade.Name;
                RemoveAsFavoriteButton.Tag = _user.Id;
                AddAsFavoriteButton.Tag = _user.Id;*/
            }
        }

        /*HISTORY*/



        /*CONTACTS*/

        private void RemoveFavoriteUI()
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
                RemoveFavoriteUI();                
            }
        }

        private void RemoveAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteRemovedSuccess == true)
            {
                PMMapContactController.RemoveFavoris(_user);
                AddFavoriteUI();
            }
        }

        private void AddRemoveFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
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
    }   
}