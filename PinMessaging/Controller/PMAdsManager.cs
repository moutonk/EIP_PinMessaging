﻿using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using PinMessaging.Utils;
using PinMessaging.View;
using PinMessaging.Other;
using PinMessaging.Model;

namespace PinMessaging.Controller
{
    class PMAdsManager
    {
        private readonly DispatcherTimer _adDisplayTime;
        private readonly PMSplashView _view;

        public PMAdsManager(double displayTime, string adPath, PMSplashView view)
        {
            _adDisplayTime = new DispatcherTimer();
            _view = view;
            InitTimer(displayTime);
            CreateAd(adPath);
        }

        private void OnTimerTick(Object sender, EventArgs args)
        {
            _adDisplayTime.Stop();

            if (RememberConnection.IsFirstConnection() == true)
                _view.MoveLogo.Begin();
            else
            {
                _view.ActiviateProgressBar();

                var pmLogInController = new PMSignInController(null, _view.InterpretResult, PinMessaging.Utils.WebService.RequestType.SignIn, PMLogInCreateStructureModel.ActionType.SignIn);
                var m = RememberConnection.GetLoginPwd();

                if (m == null)
                    _view.DisplayNextView();
                else
                    pmLogInController.LogIn(m);
            }
        }

        private static void AdClick(Object sender, EventArgs args)
        {
            var webBrowserTask = new WebBrowserTask {Uri = new Uri("http://google.com", UriKind.Absolute)};

            webBrowserTask.Show();
        }

        private void InitTimer(double durationInSec)
        {
            _adDisplayTime.Interval = TimeSpan.FromSeconds(durationInSec);
            _adDisplayTime.Tick += OnTimerTick;
        }

        public void DisplayAd()
        {
            _adDisplayTime.Start();
        }

        private void CreateAd(string adPath)
        {
            var adBitmap = new BitmapImage(new Uri(adPath, UriKind.Relative));

            var img = new Image {Source = adBitmap, Stretch = System.Windows.Media.Stretch.Fill};
            img.Tap += AdClick;

            if (_view != null)
                _view.AttachAd(img);
            else
                Logs.Error.ShowError("createAd: PMSplashView instance is null", Logs.Error.ErrorsPriority.Critical);
        }
    }
}
