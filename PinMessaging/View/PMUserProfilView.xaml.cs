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

                PointsTextBlock.Text = _user.Points;
                NbrMsgTextBlock.Text = _user.NbrMessage;
                NbrPinTextBlock.Text = _user.NbrPin;
                LoginTextBlock.Text = _user.Login;
                GradeTextBlock.Text = _user.Grade.Name;
                RemoveAsFavoriteButton.Tag = _user.Id;
                AddAsFavoriteButton.Tag = _user.Id;
            }
        }

        private void AddAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteAddedSuccess == true)
                PMMapContactController.AddNewFavoris(_user);
        }

        private void RemoveAsFavoriteButton_Post()
        {
            if (PMData.WasFavoriteRemovedSuccess == true)
                PMMapContactController.RemoveFavoris(_user);
        }

        private void AddAsFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.AddFavoriteUser, AddAsFavoriteButton_Post);

            PMData.WasFavoriteAddedSuccess = false;
            favController.AddFavoriteUser(AddAsFavoriteButton.Tag as string);
        }

        private void RemoveAsFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.RemoveFavoriteUser, RemoveAsFavoriteButton_Post);

            PMData.WasFavoriteRemovedSuccess = false;
            favController.RemoveFavoriteUser(RemoveAsFavoriteButton.Tag as string);
        }

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
                ProfilPictureImage.Source = PMData.UserProfilPicture.Source;
            }
        }
    }   
}