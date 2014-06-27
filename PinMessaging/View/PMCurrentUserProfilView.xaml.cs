using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;

namespace PinMessaging.View
{
    public partial class PMCurrentUserProfilView : PhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();

        public PMCurrentUserProfilView()
        {
            InitializeComponent();

            SetProfilPictureUI();    
            _photoChooserTask.Completed += photoChooserTask_Completed;
        }

        private void SetProfilPictureUI()
        {
            var profilPic = PMData.GetUserProfilPicture(PMData.UserId);

            if (profilPic != null)
                UserProfilImage.Source = profilPic.Img;
        }

        /*TO FINISH*/
        private void ChangeProfilPictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            _photoChooserTask.PixelWidth = 200;
            _photoChooserTask.PixelHeight = 200;
            _photoChooserTask.Show();
        }

        private async void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var pic = new PMPhotoModel {UserId = PMData.UserId, FieldBytes = new byte[e.ChosenPhoto.Length]};
                
                try
                {
                    await e.ChosenPhoto.ReadAsync(pic.FieldBytes, 0, pic.FieldBytes.Length);
                    pic.CreateStream();

                    //if the profil picture is already in the list
                    if (PMData.ProfilPicturesList.Any(img => img.UserId.Equals(PMData.UserId) == true) == true)
                    {
                        //we remove it and we add it
                        PMData.ProfilPicturesList.RemoveAll(img => img.UserId.Equals(PMData.UserId) == true);
                        PMData.ProfilPicturesList.Add(pic);
                    }
                    else
                    {
                        //or we add it
                        PMData.ProfilPicturesList.Add(pic);
                    }
                    SetProfilPictureUI();
                }
                catch (Exception exp)
                {
                    Logs.Error.ShowError("photoChooserTask_Completed", exp, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
        }
    }
}