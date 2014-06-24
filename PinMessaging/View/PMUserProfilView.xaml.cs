using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.View
{
    public partial class PMUserProfil : PhoneApplicationPage
    {
        private readonly PMUserModel _user = null;
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();

        public PMUserProfil()
        {
            InitializeComponent();

            _photoChooserTask.Completed += photoChooserTask_Completed;

            if (PMData.UserProfilPicture != null)
                ProfilPictureImage.Source = PMData.UserProfilPicture.Source;

            if (PMData.User != null)
            {
                _user = PMData.User.Clone();

                UserNameTextBlock.Text = _user.Login;

                //if the user is already in the contact list
                if (PMData.UserList.Contains(_user) == true)
                {
                    RemoveFavoriteImg();
                }

               /* PointsTextBlock.Text = _user.Points;
                NbrMsgTextBlock.Text = _user.NbrMessage;
                NbrPinTextBlock.Text = _user.NbrPin;
                LoginTextBlock.Text = _user.Login;
                GradeTextBlock.Text = _user.Grade.Name;
                RemoveAsFavoriteButton.Tag = _user.Id;
                AddAsFavoriteButton.Tag = _user.Id;*/
            }
        }

        private void RemoveFavoriteImg()
        {
            ContactImage.Source = new BitmapImage(new Uri("/Images/Icons/flag_orange_icon@2x.png", UriKind.Relative));
        }

        private void AddFavoriteImg()
        {
            ContactImage.Source = new BitmapImage(new Uri("/Images/Icons/contact_orange_icon.png", UriKind.Relative));
        }

        private void AddAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteAddedSuccess == true)
            {
                PMMapContactController.AddNewFavoris(_user);
                RemoveFavoriteImg();                
            }
        }

        private void RemoveAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteRemovedSuccess == true)
            {
                PMMapContactController.RemoveFavoris(_user);
                AddFavoriteImg();
            }
        }

        private void AddRemoveFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_user != null)
            {
                //if the user is already in the contact list
                if (PMData.UserList.Contains(_user) == true)
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

        /*TO FINISH*/
        private void ChangeProfilPictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            _photoChooserTask.Show();
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var bmp = new BitmapImage();

                bmp.SetSource(e.ChosenPhoto);
                PMData.UserProfilPicture.Source = bmp;
               // ProfilPictureImage.Source = PMData.UserProfilPicture.Source;
            }
        }
    }   
}