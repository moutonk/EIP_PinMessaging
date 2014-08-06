using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

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

            if (PMData.User != null)
            {
                LoginTextBlock.Text = PMData.User.Pseudo;
                PointsTextBlock.Text = PMData.User.Points;
                PinsCreatedTextBlock.Text = PMData.User.NbrPin;
                CommentsTextBlock.Text = PMData.User.NbrMessage;

                var gradeInfos = Utils.Utils.GetGradeInfo(PMData.User.Grade.Type);
                if (gradeInfos != null)
                {
                    GradeTextBlock.Text = gradeInfos.Item1;
                    BestBadgeInfoTextBlock.Text = gradeInfos.Item2;
                    //img
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var pivot = "";

            if (NavigationContext.QueryString.TryGetValue("open", out pivot))
            {
                switch (int.Parse(pivot))
                {
                    case 0:
                        ShowPivotNumber(0);
                        break;

                    case 1:
                        ShowPivotNumber(1);
                        break;

                    case 2:
                        ShowPivotNumber(2);
                        break;
                }
            }
        }

        private void ShowPivotNumber(int num)
        {
            if (num >= 0 && num < PivotProfil.Items.Count)
                PivotProfil.SelectedIndex = num;
        }

        private void SetProfilPictureUI()
        {
            var profilPic = PMData.GetUserProfilPicture(PMData.CurrentUserId);

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
                var pic = new PMPhotoModel { UserId = PMData.CurrentUserId, FieldBytes = new byte[e.ChosenPhoto.Length] };
                
                try
                {
                    await e.ChosenPhoto.ReadAsync(pic.FieldBytes, 0, pic.FieldBytes.Length);
                    pic.CreateStream();

                    //if the profil picture is already in the list
                    if (PMData.ProfilPicturesList.Any(img => img.UserId.Equals(PMData.CurrentUserId) == true) == true)
                    {
                        //we remove it and we add it
                        PMData.ProfilPicturesList.RemoveAll(img => img.UserId.Equals(PMData.CurrentUserId) == true);
                        PMData.ProfilPicturesList.Add(pic);
                    }
                    else
                    {
                        //or we add it
                        PMData.ProfilPicturesList.Add(pic);
                    }

                    var userController = new PMUserController(RequestType.ProfilPicture, UploadProfilPictureUpdateUi);
                    userController.UploadProfilPicture(System.Text.Encoding.UTF8.GetString(pic.FieldBytes, 0, pic.FieldBytes.Length));

                    SetProfilPictureUI();
                }
                catch (Exception exp)
                {
                    Logs.Error.ShowError("photoChooserTask_Completed", exp, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
        }

        private void UploadProfilPictureUpdateUi()
        {
            Logs.Output.ShowOutput("upload done");
        }
    }
}