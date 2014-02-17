using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMPinController : PMWebServiceEndDetector
    {
        private static readonly Dictionary<PMPinModel.PinsType, BitmapImage> PinsMapImg = new Dictionary<PMPinModel.PinsType, BitmapImage>()
        {
            {PMPinModel.PinsType.PublicMessage, Design.CreateImage(new Uri(Paths.PinPublicMessage.ToString(), UriKind.Relative))},
            {PMPinModel.PinsType.PrivateMessage, Design.CreateImage(new Uri(Paths.PinPrivateMessage.ToString(), UriKind.Relative))},
            {PMPinModel.PinsType.Eye, Design.CreateImage(new Uri(Paths.PinEye.ToString(), UriKind.Relative))},
            {PMPinModel.PinsType.Event, Design.CreateImage(new Uri(Paths.PinEvent.ToString(), UriKind.Relative))},
            {PMPinModel.PinsType.PointOfInterest, Design.CreateImage(new Uri(Paths.PinPointOfInterest.ToString(), UriKind.Relative))},
            {PMPinModel.PinsType.CourseLastStep, Design.CreateImage(new Uri(Paths.PinCourseLastStep.ToString(), UriKind.Relative))},
        };

        private static void ConvertTypeToEnumAndImg(PMPinModel pin)
        {
            if (pin.Type != null)
            {
                if (pin.Type.Equals("Event"))
                    pin.PinTypeEnum = PMPinModel.PinsType.Event;
                else if (pin.Type.Equals("Eye"))
                    pin.PinTypeEnum = PMPinModel.PinsType.Eye;                    
                else if (pin.Type.Equals("Lol"))
                    pin.PinTypeEnum = PMPinModel.PinsType.PointOfInterest;
                else
                    pin.PinTypeEnum = PMPinModel.PinsType.PrivateMessage;
            }         
        }

        private static void ConvertGeoPosToInteger(PMPinModel pin)
        {
            try
            {
                pin.GeoCoord = new GeoCoordinate(Double.Parse(pin.Location["latitude"], CultureInfo.InvariantCulture),
                                                 Double.Parse(pin.Location["longitude"], CultureInfo.InvariantCulture));
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError(exp, Logs.Error.ErrorsPriority.NotCritical);
                pin.GeoCoord = new GeoCoordinate(0, 0);
            }
        }

        public static void CompleteDataMember(PMPinModel pin)
        {
            ConvertGeoPosToInteger(pin);
            ConvertTypeToEnumAndImg(pin);
            pin.PinImg = new Image {Source = PinsMapImg[pin.PinTypeEnum]};
            pin.PinImg.Tap += pin.img_Tap;
        }

        public void GetPins(double latitude, double longitude)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"longitude", longitude.ToString(CultureInfo.InvariantCulture)},
                {"latitude", latitude.ToString(CultureInfo.InvariantCulture)},
                {"radius", "90"}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPins, SyncType.Async, dictionary, null);

            StartTimer();
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                StopTimer();

                switch (CurrentRequestType)
                {
                    case RequestType.GetPins:
                        foreach (var pin in PMData.PinsList)
                            PMMapPinController.AddPinToMap(pin);
                        PMData.PinsList.Clear();
                        break;
                    case RequestType.CreatePin:
                        break;
                }
            }
        }
    }
}
