using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        public PMPinController()
        {
            
        }

        public PMPinController(RequestType currentRequestType, Action updateUi)
        {
            CurrentRequestType = currentRequestType;
            _updateUiMethod = updateUi;
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

        public void CreatePin(Geoposition geoPos, PMPinModel pin)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"longitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Longitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"latitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Latitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"title", pin.Title},
                {"content", pin.Content},
                {"type", pin.PinTypeEnum.ToString()}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePin, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void CreatePinMessage(string pinId, PMPinModel.PinsContentType type, string content)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"pinId", pinId},
                {"type", ((int)type).ToString()},
                {"content", content}
            };  

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePinMessage, SyncType.Async, dictionary, null);

            StartTimer();
        }


        private void AddPinUiAndCode()
        {
            foreach (var pin in PMData.PinsListToAdd.Where(pin => PMMapPinController.IsPinUnique(pin) == true))
            {
                PMMapPinController.AddPinToMap(pin);
                PMData.PinsList.Add(pin);
            }
            PMData.PinsListToAdd.Clear();
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
                        AddPinUiAndCode();
                        break;
                    case RequestType.CreatePin:
                        AddPinUiAndCode();
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                    case RequestType.CreatePinMessage:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;

                }
                Logs.Output.ShowOutput("PinListSize: " + PMData.PinsList.Count);
            }
        }
    }
}
