using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using PinMessaging.Other;

namespace PinMessaging.View
{
    public partial class PMCurrentUserProfilView : PhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();

        public PMCurrentUserProfilView()
        {
            InitializeComponent();

            _photoChooserTask.Completed += photoChooserTask_Completed;
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