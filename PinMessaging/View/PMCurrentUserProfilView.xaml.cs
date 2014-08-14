using System;
using System.Linq;
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
    public partial class PMCurrentUserProfilView : PhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooserTask = new PhotoChooserTask();

        public PMCurrentUserProfilView()
        {
            InitializeComponent();

            if (Design.ProfilPictureUpdateUi(UserProfilImage, PMData.CurrentUserId) == false)
            {
                PMData.UserId = PMData.CurrentUserId;
                var userController = new PMUserController(RequestType.ProfilPicture, ProfilPictureUpdateUi);
                userController.DownloadProfilPicture(PMData.CurrentUserId);
            }

            _photoChooserTask.Completed += photoChooserTask_Completed;

            if (PMData.User == null)
            {
                Logs.Error.ShowError("PMCurrentUserProfilView: user is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            LoginTextBlock.Text = PMData.User.Pseudo;
            PointsTextBlock.Text = PMData.User.Points;
            PinsCreatedTextBlock.Text = PMData.User.NbrPin;
            CommentsTextBlock.Text = PMData.User.NbrMessage;

            var gradeInfos = Utils.Utils.GetGradeInfo(PMData.User.Grade.Type);
            
            if (gradeInfos == null)
            {
                Logs.Error.ShowError("PMCurrentUserProfilView: gradeInfos is null", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }
         
            GradeTextBlock.Text = gradeInfos.Item1;
            BestBadgeInfoTextBlock.Text = gradeInfos.Item2;
            BadgeImgUpdateUi(PMData.User.Grade.Type);
        }

        private void BadgeImgUpdateUi(PMGradeModel.GradeType type)
        {
            var tmpType = PMGradeModel.GradeType.PointBronze;
            var imgs = new []
            {
                PointBronzeImage, PointArgentImage, PointOrImage,
                Pin50Image, Message50Image, BetaTesterImage
            };

            while (tmpType <= type && (int)tmpType < imgs.Length)
            {
                imgs[(int)tmpType].Source = new BitmapImage(new Uri("/Images/Icons/cup_orange_icon@2x.png", UriKind.Relative));
                tmpType++;
            }
        }

        private void ProfilPictureUpdateUi()
        {
            Design.ProfilPictureUpdateUi(UserProfilImage, PMData.CurrentUserId);
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

        /*TO FINISH*/
        private void ChangeProfilPictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            _photoChooserTask.PixelWidth = 200;
            _photoChooserTask.PixelHeight = 200;
            _photoChooserTask.Show();
        }

        private async void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK)
            {
                Logs.Error.ShowError("photoChooserTask_Completed: TaskResult is not OK", Logs.Error.ErrorsPriority.NotCritical);
                return;
            }

            var pic = new PMPhotoModel { UserId = PMData.CurrentUserId, FieldBytes = new byte[e.ChosenPhoto.Length] };
                
            try
            {
                await e.ChosenPhoto.ReadAsync(pic.FieldBytes, 0, pic.FieldBytes.Length);
                Logs.Output.ShowOutput("####################################: " + pic.FieldBytes.Length);

                //if the profil picture is already in the list
                if (PMData.ProfilPicturesList.Any(img => img.UserId.Equals(PMData.CurrentUserId) == true) == true)
                {
                    //we remove it
                    PMData.ProfilPicturesList.RemoveAll(img => img.UserId.Equals(PMData.CurrentUserId) == true);
                }
                PMData.ProfilPicturesList.Add(pic);

                var userController = new PMUserController(RequestType.ProfilPicture, UploadProfilPictureUpdateUi);
                    
                userController.UploadProfilPicture(Convert.ToBase64String(pic.FieldBytes)/* System.Text.Encoding.UTF8.GetString(pic.FieldBytes, 0, pic.FieldBytes.Length)*/);

                Design.ProfilPictureUpdateUi(UserProfilImage, PMData.CurrentUserId);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("photoChooserTask_Completed", exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void UploadProfilPictureUpdateUi()
        {
            Logs.Output.ShowOutput("upload done");
        }
    }
}