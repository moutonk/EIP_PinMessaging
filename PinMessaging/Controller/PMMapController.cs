using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PinMessaging.Controller
{
    class PMMapController
    {
        
    }

    public static class PMMapPushpinController
    {
        public static MapLayer mapLayer {get; set;}
        public static Dictionary<SolidColorBrush, WriteableBitmap> coloredPins {get; set;}

        public static void Initialization()
        {
            coloredPins = new Dictionary<SolidColorBrush, WriteableBitmap>();

            coloredPins[App.Current.Resources["PMOrange"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.LogoSplashBlack.ToString(), UriKind.Relative), App.Current.Resources["PMOrange"] as SolidColorBrush);
            coloredPins[App.Current.Resources["PMGreen"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.LogoSplashBlack.ToString(), UriKind.Relative), App.Current.Resources["PMGreen"] as SolidColorBrush);
            coloredPins[App.Current.Resources["PMPurple"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.LogoSplashBlack.ToString(), UriKind.Relative), App.Current.Resources["PMPurple"] as SolidColorBrush);
            coloredPins[App.Current.Resources["PMYellow"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.LogoSplashBlack.ToString(), UriKind.Relative), App.Current.Resources["PMYellow"] as SolidColorBrush);
        }

        public static void AddPushpinToMap(PMMapPushpinModel pin)
        {
            MapOverlay overlay = new MapOverlay();

            pin.pinImg.Tap += img_Tap;
            pin.pinImg.Source = coloredPins[App.Current.Resources["PMOrange"] as SolidColorBrush];
         
            overlay.Content = pin.pinImg;  //pin.pushpin;
            overlay.GeoCoordinate = pin.geoCoord;

            //pin.pushpin.Background = DeterminePinBackgroundColor(pin);

            if (mapLayer != null)
                mapLayer.Add(overlay);
        }

        static void img_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Debug.WriteLine("ici");
        }

        public static void RemovePushpinFromMap(PMMapPushpinModel pin)
        {
            if (mapLayer != null)
            {

            }
        }

        private static SolidColorBrush DeterminePinBackgroundColor(PMMapPushpinModel pin)
        {
            switch (pin.pinType)
            {
                case PinMessaging.Model.PMMapPushpinModel.PinType.PublicMessage:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;
                    break;

                case PinMessaging.Model.PMMapPushpinModel.PinType.PrivateMessage:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;
                    break;

                case PinMessaging.Model.PMMapPushpinModel.PinType.Event:
                    return App.Current.Resources["PMPurple"] as SolidColorBrush;
                    break;

                case PinMessaging.Model.PMMapPushpinModel.PinType.TreasureHunt:
                    return App.Current.Resources["PMYellow"] as SolidColorBrush;
                    break;

                case PinMessaging.Model.PMMapPushpinModel.PinType.TouristInfo:
                    return App.Current.Resources["PMGreen"] as SolidColorBrush;
                    break;

                default:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;
                    break;
            }
            return App.Current.Resources["PMOrange"] as SolidColorBrush;
        }
    }
}
