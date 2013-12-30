using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using PinMessaging.Controller;
using PinMessaging.Utils;
using System.Windows;
using PinMessaging.Other;

namespace PinMessaging.View
{
    public partial class PMSplashView : PhoneApplicationPage
    {
        private PMAdsManager _adsManager;

        public PMSplashView()
        {
            InitializeComponent();


            // RememberConnection.ResetAll();

            //if (RememberConnection.IsFirstConnection() == true)
            //    MessageBox.Show(AppResources.WelcomeSentence);

            ManageIntersticialAd(Paths.LogoSplashOrange.ToString(), 1);
            _adsManager.DisplayAd();

            //_adsManager.hideAd();
        }

        public void ManageIntersticialAd(string adPath, double displayTime)
        {

            _adsManager = new PMAdsManager(displayTime, adPath, this);
        }

        public void AttachAd(Image ad)
        {
            if (GridLogo != null)
                GridLogo.Children.Add(ad);
            else
                Logs.Error.ShowError("attachAd: GridLogo is null", ErrorsPriority.NotCritical);
        }

        public void ActiviateProgressBar()
        {
            ProgressBar.IsIndeterminate = true;
            ProgressBar.Visibility = Visibility.Visible;
        }

        public bool InterpretResult()
        {
            if (PMData.IsSignInSuccess)
                DisplayMapView();
            else
                DisplayNextView();
            return true;
        }

        public bool DisplayNextView()
        {
            try
            {
                NavigationService.Navigate(Paths.FirstLaunch);
                return true;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, ErrorsPriority.Critical);
                return false;
            }
        }

        public bool DisplayMapView()
        {
            try
            {
                NavigationService.Navigate(Paths.MapView);
                return true;
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, ErrorsPriority.Critical);
                return false;
            }
        }

        private void MoveLogo_OnCompleted(object sender, EventArgs e)
        {
            DisplayNextView();
        }
    }
}