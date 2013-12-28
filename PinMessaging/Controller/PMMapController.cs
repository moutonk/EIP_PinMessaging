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

    public static class PMMapPushpinController
    {
        private static readonly Dictionary<PMMapPushpinModel.PinsType, BitmapImage> PinsMap = new Dictionary<PMMapPushpinModel.PinsType, BitmapImage>()
        {
            {PMMapPushpinModel.PinsType.PublicMessage, Design.CreateImage(new Uri(Paths.PinPublicMessage.ToString(), UriKind.Relative))},
            {PMMapPushpinModel.PinsType.PrivateMessage, Design.CreateImage(new Uri(Paths.PinPrivateMessage.ToString(), UriKind.Relative))},
            {PMMapPushpinModel.PinsType.Eye, Design.CreateImage(new Uri(Paths.PinEye.ToString(), UriKind.Relative))},
            {PMMapPushpinModel.PinsType.Event, Design.CreateImage(new Uri(Paths.PinEvent.ToString(), UriKind.Relative))},
            {PMMapPushpinModel.PinsType.PointOfInterest, Design.CreateImage(new Uri(Paths.PinPointOfInterest.ToString(), UriKind.Relative))},
            {PMMapPushpinModel.PinsType.CourseLastStep, Design.CreateImage(new Uri(Paths.PinCourseLastStep.ToString(), UriKind.Relative))},
        };

        public static void AddPushpinToMap(PMMapPushpinModel pin)
        {
            var overlay = new MapOverlay();

            pin.PinImg.Tap += pin.img_Tap;
            pin.PinImg.Source = PinsMap[pin.PinType];

            //center the mapoverlay, will change later
            overlay.PositionOrigin = new Point(0.4, 1);
            overlay.Content = pin.PinImg;
            overlay.GeoCoordinate = pin.GeoCoord;

            if (PMData.MapLayerContainer != null)
                PMData.MapLayerContainer.Add(overlay);
        }

        public static void RemovePushpinFromMapLayer(PMMapPushpinModel pin)
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
