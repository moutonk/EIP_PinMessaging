using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PinMessaging.Model;
using PinMessaging.Resources;
using PinMessaging.Utils;
using PinMessaging.Other;

namespace PinMessaging.View
{
    public partial class PMFirstLaunchView : PhoneApplicationPage
    {
        public PMFirstLaunchView()
        {
            InitializeComponent();

            if (GridLogo != null)
            {
                Img.ImageSource = new BitmapImage(new Uri(Paths.LogoSplashBlack.ToString(), UriKind.Relative));
            }
            else
                ErrorsManager.ShowError("createAd: PMFirstLaunchView instance is null", ErrorsPriority.Critical);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void ButtonYesClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PhoneApplicationService.Current.State[Paths.ApplicationDico.SignInUpParams] = new PMLogInCreateStructureModel(AppResources.SignInPageTitle, AppResources.PMEmail, AppResources.ButtonValidate, PMLogInCreateStructureModel.ActionType.SignIn);
                NavigationService.Navigate(Paths.SignInCreate);
            }
            catch (Exception exp)
            {
                ErrorsManager.ShowError(exp, ErrorsPriority.Critical);
            }
        }

        private void ButtonNoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PhoneApplicationService.Current.State[Paths.ApplicationDico.SignInUpParams] = new PMLogInCreateStructureModel(AppResources.CreateAccountPageTitle, AppResources.PMEmail, AppResources.ButtonValidate, PMLogInCreateStructureModel.ActionType.Create);
                NavigationService.Navigate(Paths.SignInCreate);
            }
            catch (Exception exp)
            {
                ErrorsManager.ShowError(exp, ErrorsPriority.Critical);
            }
        }   
    }
}