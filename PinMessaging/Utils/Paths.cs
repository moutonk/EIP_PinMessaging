using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using PinMessaging.Model;

namespace PinMessaging.Utils
{
    class Paths
    {
        public static readonly Uri LogInView = new Uri("/View/Logos/PMLogInView.xaml", UriKind.Relative);
        public static readonly Uri LogoSplashOrange = new Uri("/Images/Logos/logo_orange.png", UriKind.Relative);
        public static readonly Uri LogoSplashBlack = new Uri("/Images/Logos/logo_black.png", UriKind.Relative);
        public static readonly Uri MapView = new Uri("/View/PMMapView.xaml", UriKind.Relative);   
        public static readonly Uri FirstLaunch = new Uri("/View/PMFirstLaunchView.xaml", UriKind.Relative);
        public static readonly Uri SignInCreate = new Uri("/View/PMLogInCreateView.xaml", UriKind.Relative);
        public static readonly Uri SettingsView = new Uri("/View/PMSettingsView.xaml", UriKind.Relative);

        public static readonly string ServerAddress = "http://163.5.84.244/Spring/"; //alexis: 192.168.1.6 serveur_lapbeip:163.5.84.244/Spring/ suivi:163.5.84.52

        public static readonly Uri PinEventIcon = new Uri("/Images/Pins/event_icon.png", UriKind.Relative);
        public static readonly Uri PinEventIconIntermediate = new Uri("/Images/Pins/event_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateEventIcon = new Uri("/Images/Pins/private_event_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateEventIconIntermediate = new Uri("/Images/Pins/private_event_icon@2x.png", UriKind.Relative);

        public static readonly Uri PinViewIcon = new Uri("/Images/Pins/view_icon.png", UriKind.Relative);
        public static readonly Uri PinViewIconIntermediate = new Uri("/Images/Pins/view_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateViewIcon = new Uri("/Images/Pins/private_view_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateViewIconIntermediate = new Uri("/Images/Pins/private_view_icon@2x.png", UriKind.Relative);

        public static readonly Uri PinCourseStartIcon = new Uri("/Images/Pins/course_start_icon.png", UriKind.Relative);
        public static readonly Uri PinCourseStartIconIntermediate = new Uri("/Images/Pins/course_start_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseStartIcon = new Uri("/Images/Pins/private_course_start_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseStartIconIntermediate = new Uri("/Images/Pins/private_course_start_icon@2x.png", UriKind.Relative);

        public static readonly Uri PinCourseNextStepIcon = new Uri("/Images/Pins/course_next_step_icon.png", UriKind.Relative);
        public static readonly Uri PinCourseNextStepIconIntermediate = new Uri("/Images/Pins/course_next_step_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseNextStepIcon = new Uri("/Images/Pins/private_course_next_step_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseNextStepIconIntermediate = new Uri("/Images/Pins/private_course_next_step_icon@2x.png", UriKind.Relative);

        public static readonly Uri PinCourseLastStepIcon = new Uri("/Images/Pins/course_last_step_icon.png", UriKind.Relative);
        public static readonly Uri PinCourseLastStepIconIntermediate = new Uri("/Images/Pins/course_last_step_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseLastStepIcon = new Uri("/Images/Pins/private_course_last_step_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateCourseLastStepIconIntermediate = new Uri("/Images/Pins/private_course_last_step_icon@2x.png", UriKind.Relative);

        public static readonly Uri PinPublicMessageIcon = new Uri("/Images/Pins/message_icon.png", UriKind.Relative);
        public static readonly Uri PinPublicMessageIconIntermediate = new Uri("/Images/Pins/message_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinPrivateMessageIcon = new Uri("/Images/Pins/private_message_icon.png", UriKind.Relative);
        public static readonly Uri PinPrivateMessageIconIntermediate = new Uri("/Images/Pins/private_message_icon@2x.png", UriKind.Relative);
  
        
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

        public static readonly Uri TargetButton = new Uri("/Images/Menu/target@2x.png", UriKind.Relative);
        public static readonly Uri MenuButton = new Uri("/Images/Menu/menu_icon_2@2x.png", UriKind.Relative);
        public static readonly Uri NotificationsButton = new Uri("/Images/Icons/flag_orange_icon@2x.png", UriKind.Relative);
        public static readonly Uri ContactsButton = new Uri("/Images/Icons/contact_orange_icon@2x.png", UriKind.Relative);
        public static readonly Uri PinsButton = new Uri("/Images/Icons/logo_flat_orange@2x.png", UriKind.Relative);

        public static readonly Uri LeftMenuMap = new Uri("/Images/Icons/map_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuFilters = new Uri("/Images/Icons/filer_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuProfil = new Uri("/Images/Icons/profile_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuPins = new Uri("/Images/Icons/logo_flat_white@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuSettings = new Uri("/Images/Icons/settings_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuRewards = new Uri("/Images/Icons/cup_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuAbout = new Uri("/Images/Icons/about_white_icon@2x.png", UriKind.Relative);
        public static readonly Uri LeftMenuLogout = new Uri("/Images/Icons/cross_white_icon@2x.png", UriKind.Relative);

        public static readonly Uri CreatePinPublicIcon = new Uri("/Images/Icons/white_planet.png", UriKind.Relative);
        public static readonly Uri CreatePinPrivateIcon = new Uri("/Images/Icons/white_lock.png", UriKind.Relative);

        public static Dictionary<PMPinModel.PinsType, BitmapImage> PinsMapImg;

        static Paths()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                PinsMapImg = new Dictionary<PMPinModel.PinsType, BitmapImage>()
                {
                    {PMPinModel.PinsType.PublicMessage, Design.CreateImage(new Uri(PinPublicMessageIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateMessage, Design.CreateImage(new Uri(PinPrivateMessageIconIntermediate.ToString(), UriKind.Relative))},
                    
                    {PMPinModel.PinsType.View, Design.CreateImage(new Uri(PinViewIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateView, Design.CreateImage(new Uri(PinPrivateViewIconIntermediate.ToString(), UriKind.Relative))},
                    
                    {PMPinModel.PinsType.Event, Design.CreateImage(new Uri(PinEventIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateEvent, Design.CreateImage(new Uri(PinPrivateEventIconIntermediate.ToString(), UriKind.Relative))},
                    
                    {PMPinModel.PinsType.CourseLastStep, Design.CreateImage(new Uri(PinCourseLastStepIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateCourseLastStep, Design.CreateImage(new Uri(PinPrivateCourseLastStepIconIntermediate.ToString(), UriKind.Relative))},

                    {PMPinModel.PinsType.CourseStart, Design.CreateImage(new Uri(PinCourseStartIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateCourseStart, Design.CreateImage(new Uri(PinPrivateCourseStartIconIntermediate.ToString(), UriKind.Relative))},

                    {PMPinModel.PinsType.CourseNextStep, Design.CreateImage(new Uri(PinCourseNextStepIconIntermediate.ToString(), UriKind.Relative))},
                    {PMPinModel.PinsType.PrivateCourseNextStep, Design.CreateImage(new Uri(PinPrivateCourseNextStepIconIntermediate.ToString(), UriKind.Relative))},
                };
            });
            
        }


        public static class ApplicationDico
        {
            public static readonly string SignInUpParams = "signInUpParams";
        }
    }
}
