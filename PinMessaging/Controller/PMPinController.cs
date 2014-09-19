using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.Devices.Geolocation;
using PinMessaging.Model;
using PinMessaging.Other;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Controller
{
    class PMPinController : PMWebServiceEndDetector
    {
        private Action _updateUiMethod;

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

        public void GetPinsUser()
        {
            var dictionary = new Dictionary<string, string>();

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPinsUser, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void CreatePin(Geoposition geoPos, PMPinModel pin)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"longitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Point.Position.Longitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"latitude", Utils.Utils.ConvertDoubleCommaToPoint(geoPos.Coordinate.Point.Position.Latitude.ToString()).ToString(CultureInfo.InvariantCulture)},
                {"title", pin.Title},
                {"contentType", ((int)pin.ContentType).ToString()},
                {"content", pin.Content},
                {"pinType", ((int)pin.PinType).ToString()},
                {"private", pin.Private == true ? "true" : "false"},
                {"authorisedUsersId", pin.AuthoriseUsersId}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.CreatePin, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void GetPinMessage(PMPinModel pin)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"pinId", pin.Id},
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.GetPinMessages, SyncType.Async, dictionary, null);

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

        public void DeletePin(string pinId)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"pinId", pinId}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.DeletePin, SyncType.Async, dictionary, null);

            StartTimer();
        }

        public void ChangePin(string pinId, string title, PMPinModel.PinsContentType contentType, string content)
        {
            var dictionary = new Dictionary<string, string>
            {
                {"pinId", pinId},
                {"title", title},
                {"contentType", ((int)contentType).ToString()},
                {"content", content}
            };

            PMWebService.SendRequest(HttpRequestType.Post, RequestType.ChangePin, SyncType.Async, dictionary, null);

            StartTimer();
        }

        private static void AddPinUiAndCode()
        {
            try
            {
                foreach (var pin in PMData.PinsListToAdd.Where(pin => PMMapPinController.IsPinUnique(pin) == true))
                {
                    PMMapPinController.AddPinToMap(pin);
                    PMData.PinsList.Add(pin);
                }
                PMData.PinsListToAdd.Clear();
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("AddPinUiAndCode: " + exp.Message, exp, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        protected override void waitEnd_Tick(object sender, EventArgs e)
        {
            if (PMWebService.OnGoingRequest == false)
            {
                //when we are here it means that the webservice call is over and the data are parsed and stocked in PMData
                StopTimer();

                Logs.Output.ShowOutput(CurrentRequestType.ToString());

                switch (CurrentRequestType)
                {
                    case RequestType.GetPins:
                        AddPinUiAndCode();
                        if (_updateUiMethod != null)
                            _updateUiMethod();
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
                    case RequestType.GetPinMessages:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                    case RequestType.DeletePin:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;
                    case RequestType.ChangePin:
                        if (_updateUiMethod != null)
                            _updateUiMethod();
                        break;

                }
                _updateUiMethod = null;
                Logs.Output.ShowOutput("PinListSize: " + PMData.PinsList.Count);
            }
        }
    }
}
