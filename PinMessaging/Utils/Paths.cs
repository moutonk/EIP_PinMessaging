﻿using System;

namespace PinMessaging.Utils
{
    static class Paths
    {
        public static readonly Uri LogInView = new Uri("/View/Logos/PMLogInView.xaml", UriKind.Relative);
        public static readonly Uri LogoSplashOrange = new Uri("/Images/Logos/logo_orange.png", UriKind.Relative);
        public static readonly Uri LogoSplashBlack = new Uri("/Images/Logos/logo_black.png", UriKind.Relative);
        public static readonly Uri MapView = new Uri("/View/PMMapView.xaml", UriKind.Relative);   
        public static readonly Uri FirstLaunch = new Uri("/View/PMFirstLaunchView.xaml", UriKind.Relative);
        public static readonly Uri SignInCreate = new Uri("/View/PMLogInCreateView.xaml", UriKind.Relative);

        public static readonly string ServerAddress = "http://163.5.84.244/Spring/"; //alexis: 192.168.1.10:8080 163.5.84.244/Spring/ suivi:163.5.84.52

        public static readonly Uri PinEvent = new Uri("/Images/Pins/event.png", UriKind.Relative);
        public static readonly Uri PinEye = new Uri("/Images/Pins/eye.png", UriKind.Relative);
        public static readonly Uri PinCourseLastStep = new Uri("/Images/Pins/course_last_step.png", UriKind.Relative);
        public static readonly Uri PinPointOfInterest = new Uri("/Images/Pins/poi.png", UriKind.Relative);
        public static readonly Uri PinPublicMessage = new Uri("/Images/Pins/public_message.png", UriKind.Relative);
        public static readonly Uri PinPrivateMessage = new Uri("/Images/Pins/private_message.png", UriKind.Relative);

        public static readonly Uri FlagAR = new Uri("/Images/Flags/AR.png", UriKind.Relative);
        public static readonly Uri FlagCN = new Uri("/Images/Flags/CN.png", UriKind.Relative);
        public static readonly Uri FlagDE = new Uri("/Images/Flags/DE.png", UriKind.Relative);
        public static readonly Uri FlagEN = new Uri("/Images/Flags/EN.png", UriKind.Relative);
        public static readonly Uri FlagES = new Uri("/Images/Flags/ES.png", UriKind.Relative);
        public static readonly Uri FlagFI = new Uri("/Images/Flags/FI.png", UriKind.Relative);
        public static readonly Uri FlagFR = new Uri("/Images/Flags/FR.png", UriKind.Relative);
        public static readonly Uri FlagIN = new Uri("/Images/Flags/IN.png", UriKind.Relative);
        public static readonly Uri FlagIT = new Uri("/Images/Flags/IT.png", UriKind.Relative);
        public static readonly Uri FlagJP = new Uri("/Images/Flags/JP.png", UriKind.Relative);
        public static readonly Uri FlagKR = new Uri("/Images/Flags/KR.png", UriKind.Relative);
        public static readonly Uri FlagNO = new Uri("/Images/Flags/NO.png", UriKind.Relative);
        public static readonly Uri FlagOT = new Uri("/Images/Flags/OT.png", UriKind.Relative);
        public static readonly Uri FlagPT = new Uri("/Images/Flags/PT.png", UriKind.Relative);
        public static readonly Uri FlagRU = new Uri("/Images/Flags/RU.png", UriKind.Relative);
        public static readonly Uri FlagSE = new Uri("/Images/Flags/SE.png", UriKind.Relative);

        public static class ApplicationDico
        {
            public static readonly string SignInUpParams = "signInUpParams";
        }
    }
}