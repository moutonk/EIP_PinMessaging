using System.Windows;
using Microsoft.Phone.Controls;
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

        public PMUserProfil()
        {
            InitializeComponent();

            if (PMData.User != null)
            {
                _user = PMData.User.Clone();

                PointsTextBlock.Text = _user.Points;
                NbrMsgTextBlock.Text = _user.NbrMessage;
                NbrPinTextBlock.Text = _user.NbrPin;
                LoginTextBlock.Text = _user.Login;
                GradeTextBlock.Text = _user.Grade;
                RemoveAsFavoriteButton.Tag = _user.Id;
                AddAsFavoriteButton.Tag = _user.Id;
            }
        }

        private void AddAsFavoriteButton_Post()
        {
            Logs.Output.ShowOutput("FAV ADD FINI: " + PMData.WasFavoriteAddedSuccess.ToString());
            if (PMData.WasFavoriteAddedSuccess == true)
                PMMapContactController.AddNewFavoris(_user);
        }

        private void RemoveAsFavoriteButton_Post()
        {
            Logs.Output.ShowOutput("FAV REM FINI: " + PMData.WasFavoriteAddedSuccess.ToString());
            if (PMData.WasFavoriteRemovedSuccess == true)
                PMMapContactController.RemoveFavoris(_user);
        }

        private void AddAsFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.AddFavoriteUser, AddAsFavoriteButton_Post);

            favController.AddFavoriteUser(AddAsFavoriteButton.Tag as string);
        }

        private void RemoveAsFavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var favController = new PMFavoriteController(RequestType.RemoveFavoriteUser, RemoveAsFavoriteButton_Post);

            favController.RemoveFavoriteUser(RemoveAsFavoriteButton.Tag as string);
        }
    }   
}