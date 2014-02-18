using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PinMessaging.Controller
{
    public static class PMMapController
    {
        
    }

    public static class PMMapPinController
    {
   
        public static void AddPinToMap(PMPinModel pin)
        {
            var overlay = new MapOverlay();

            //pin.PinImg.Tap += pin.img_Tap;
            //pin.PinImg.Source = PinsMap[pin.PinTypeEnum];

            //center the mapoverlay, will change later
            overlay.PositionOrigin = new Point(0.3, 1);

            StackPanel sp = new StackPanel();
            TextBlock tb = new TextBlock();
            tb.Text = pin.Location["name"];
            tb.Foreground = new SolidColorBrush(Colors.Black);

            sp.Children.Add(tb);
            sp.Children.Add(pin.PinImg);

            overlay.Content = /*pin.PinImg*/sp;
            overlay.GeoCoordinate = pin.GeoCoord;
            

            if (PMData.MapLayerContainer != null)
                PMData.MapLayerContainer.Add(overlay);
        }

        public static void RemovePushpinFromMapLayer(PMPinModel pin)
        {
            if (PMData.MapLayerContainer != null)
            {
            //    var res = from mapLayerElem in MapLayerContainer
            //              where pin == mapLayerElem.Content
            //              select mapLayerElem;
            }
        }
    }
}
