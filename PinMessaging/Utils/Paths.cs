using System;

namespace PinMessaging.Utils
{
    static class Paths
    {
        public static readonly Uri LogInView = new Uri("/View/PMLogInView.xaml", UriKind.Relative);
        public static readonly Uri MapView = new Uri("/View/PMMapView.xaml", UriKind.Relative);
        public static readonly Uri LogoSplashOrange = new Uri("/Assets/logo_orange.png", UriKind.Relative);
        public static readonly Uri LogoSplashBlack = new Uri("/Assets/logo_black.png", UriKind.Relative);
        public static readonly string ServerAddress = "http://163.5.84.244/Spring/"; //alexis: 192.168.1.10:8080 163.5.84.244/Spring/ suivi:163.5.84.52
        public static readonly Uri FirstLaunch = new Uri("/View/PMFirstLaunchView.xaml", UriKind.Relative);
        public static readonly Uri SignInCreate = new Uri("/View/PMLogInCreateView.xaml", UriKind.Relative);

        public static class ApplicationDico
        {
            public static readonly string SignInUpParams = "signInUpParams";
        }
    }
}
