using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PinMessaging.Controller
{
    class PMMapController
    {
        
    }

    public static class PMMapPushpinController
    {
        public static MapLayer MapLayer { get; set; }
        private static Dictionary<SolidColorBrush, WriteableBitmap> ColoredPins {get; set;}

        public static void Initialization()
        {
            ColoredPins = new Dictionary<SolidColorBrush, WriteableBitmap>();

            ColoredPins[App.Current.Resources["PMOrange"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.PINTEST.ToString(), UriKind.Relative), App.Current.Resources["PMOrange"] as SolidColorBrush);
            ColoredPins[App.Current.Resources["PMGreen"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.PINTEST2.ToString(), UriKind.Relative), App.Current.Resources["PMGreen"] as SolidColorBrush);
            ColoredPins[App.Current.Resources["PMPurple"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.PINTEST.ToString(), UriKind.Relative), App.Current.Resources["PMPurple"] as SolidColorBrush);
            ColoredPins[App.Current.Resources["PMYellow"] as SolidColorBrush] = Design.ChangeImageColor(new Uri(Paths.PINTEST.ToString(), UriKind.Relative), App.Current.Resources["PMYellow"] as SolidColorBrush);
        }

        public static void AddPushpinToMap(PMMapPushpinModel pin)
        {
            var overlay = new MapOverlay();

            pin.PinImg.Tap += pin.img_Tap;
            pin.PinImg.Source = ColoredPins[App.Current.Resources["PMGreen"] as SolidColorBrush];//DeterminePinBackgroundColor(pin)];

            //center the mapoverlay, will change later
            overlay.PositionOrigin = new Point(0.4, 1);
            overlay.Content = pin.PinImg;
            overlay.GeoCoordinate = pin.GeoCoord;

            if (MapLayer != null)
                MapLayer.Add(overlay);
        }

        public static void RemovePushpinFromMapLayer(PMMapPushpinModel pin)
        {
            if (MapLayer != null)
            {

            }
        }

        private static SolidColorBrush DeterminePinBackgroundColor(PMMapPushpinModel pin)
        {
            switch (pin.pinType)
            {
                case PMMapPushpinModel.PinType.PublicMessage:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;

                case PMMapPushpinModel.PinType.PrivateMessage:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;

                case PMMapPushpinModel.PinType.Event:
                    return App.Current.Resources["PMPurple"] as SolidColorBrush;

                case PMMapPushpinModel.PinType.TreasureHunt:
                    return App.Current.Resources["PMYellow"] as SolidColorBrush;

                case PMMapPushpinModel.PinType.TouristInfo:
                    return App.Current.Resources["PMGreen"] as SolidColorBrush;

                default:
                    return App.Current.Resources["PMOrange"] as SolidColorBrush;
            }
        }
    }
}
