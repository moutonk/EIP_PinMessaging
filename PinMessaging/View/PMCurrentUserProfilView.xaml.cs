using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Resources;
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

            if (PMData.User != null)
            {
                LoginTextBlock.Text = PMData.User.Login;
                PointsTextBlock.Text = PMData.User.Points;
                PinsCreatedTextBlock.Text = PMData.User.NbrPin;
                CommentsTextBlock.Text = PMData.User.NbrMessage;

                var gradeInfos = GetGradeInfo(PMData.User.Grade.Type);
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

        private Tuple<string, string, Uri> GetGradeInfo(PMGradeModel.GradeType grade)
        {
            switch (grade)
            {
                case PMGradeModel.GradeType.PointBronze:
                    return new Tuple<string, string, Uri>(AppResources.GradePointCopper, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.PointArgent:
                    return new Tuple<string, string, Uri>(AppResources.GradePointSilver, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.PointOr:
                    return new Tuple<string, string, Uri>(AppResources.GradePointGold, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.Pin50:
                    return new Tuple<string, string, Uri>(AppResources.GradePin50, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.Message50:
                    return new Tuple<string, string, Uri>(AppResources.GradeMessage50, AppResources.BadgePointCopper, null);

                case PMGradeModel.GradeType.Betatester:
                    return new Tuple<string, string, Uri>(AppResources.GradeBetaTester, AppResources.BadgePointCopper, null);
            }
            return null;
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