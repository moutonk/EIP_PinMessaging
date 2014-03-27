using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Windows.Devices.Geolocation;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;
using PinMessaging.View;

namespace PinMessaging.Controller
{
    class PMPinController : PMWebServiceEndDetector
    {
        private readonly Action _updateUiMethod;
 
        public PMPinController(RequestType currentRequestType, Action updateUi)
        {
            CurrentRequestType = currentRequestType;
            _updateUiMethod = updateUi;
        }

        private static void ConvertTypeToEnumAndImg(PMPinModel pin)
        {
            if (pin.Type != null)
            {
                if (pin.Type.Equals("Public_msg"))
                    pin.PinTypeEnum = PMPinModel.PinsType.PublicMessage;
                else if (pin.Type.Equals("Private_msg"))
                    pin.PinTypeEnum = PMPinModel.PinsType.PrivateMessage;
                else if (pin.Type.Equals("Event"))
                    pin.PinTypeEnum = PMPinModel.PinsType.Event;
                else if (pin.Type.Equals("View_point"))
                    pin.PinTypeEnum = PMPinModel.PinsType.Eye;                    
                else if (pin.Type.Equals("Interest_point"))
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

        public  void CompleteDataMember(PMPinModel pin)
        {
            ConvertGeoPosToInteger(pin);
            ConvertTypeToEnumAndImg(pin);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                pin.PinImg = new Image { Source = Paths.PinsMapImg[pin.PinTypeEnum] };
                pin.PinImg.Tap += pin.img_Tap;
            });
        }

        public void GetPins(double latitude, double longitude)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"longitude", longitude.ToString(CultureInfo.InvariantCulture)},
                {"latitude", latitude.ToString(CultureInfo.InvariantCulture)},
                {"radius", "1"} /*WILL CHANGE*/
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPins, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void CreatePin(Geoposition geoPos, string[] stringArray)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"longitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Longitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"latitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Latitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"name", stringArray[0]},
                {"description", stringArray[1]},
                {"type", stringArray[2]}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePin, SyncType.Async, dictionary, null);

            StartTimer();
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                //when we are here it means that the webservice call is over and the data are parsed and stocked in PMData
                StopTimer();

                switch (CurrentRequestType)
                {
                    case RequestType.GetPins:
                        foreach (var pin in PMData.PinsList)
                            PMMapPinController.AddPinToMap(pin);
                        PMData.PinsList.Clear();
                        break;
                    case RequestType.CreatePin:
                        foreach (var pin in PMData.PinsList)
                            PMMapPinController.AddPinToMap(pin);
                        PMData.PinsList.Clear();
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                }
            }
        }
    }
}
