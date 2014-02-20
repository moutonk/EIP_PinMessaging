using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinMessaging.Controller;
using PinMessaging.Model;
using PinMessaging.Utils;
using PinMessaging.Utils.WebService;

namespace PinMessaging.Other
{
    class PMDataConverter
    {
        public void ParseJson(string json, RequestType currentRequestType)
        {
            if (json != null)
            {
                try
                {
                    switch (currentRequestType)
                    {
                        case RequestType.SignIn:
                            var item1 = JsonConvert.DeserializeObject<JArray>(json);                 
                            PMData.IsSignInSuccess = Boolean.Parse((string) item1[0]);
                            break;

                        case RequestType.CheckEmail:
                            var item2 = JsonConvert.DeserializeObject<JArray>(json);
                            PMData.IsEmailDispo = Boolean.Parse((string) item2[0]);
                            break;

                        case RequestType.SignUp:
                            var item3 = JsonConvert.DeserializeObject<JArray>(json);
                            PMData.IsSignUpSuccess = Boolean.Parse((string) item3[0]);
                            break;

                        case RequestType.GetPins:
                            ParseGetPins(json);
                            break;

                        case RequestType.CreatePin:
                            ParseCreatePin(json);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Logs.Error.ShowError(e, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
        }

        private void ParseCreatePin(string json)
        {
           try
            {
                var pin = JsonConvert.DeserializeObject<PMPinModel>(json);
                var pinController = new PMPinController();

                pinController.CompleteDataMember(pin);
                pin.ShowPinContent();

                Deployment.Current.Dispatcher.BeginInvoke(() => PMMapPinController.AddPinToMap(pin));
                PMData.AddToQueuePinsList(pin);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseCreatePins: could not get the pins for the following reason", exp, Logs.Error.ErrorsPriority.NotCritical);

                try
                {
                    var item = JsonConvert.DeserializeObject<JArray>(json);

                    Logs.Error.ShowError("error " + (string)item[1], Logs.Error.ErrorsPriority.NotCritical);
        
                }
                catch (Exception exp2)
                {
                    Logs.Error.ShowError("ParseCreatePins: could not get the error message", exp2, Logs.Error.ErrorsPriority.NotCritical);
                } 
            }
        }

        public static void ParseGetPins(string json)
        {
            try
            {
                var pinCollection = JsonConvert.DeserializeObject<List<PMPinModel>>(json);
                var pinController = new PMPinController();

                foreach (var pmMapPushpinModel in pinCollection)
                {
                    pinController.CompleteDataMember(pmMapPushpinModel);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        PMMapPinController.AddPinToMap(pmMapPushpinModel);         
                    });
                    pmMapPushpinModel.ShowPinContent();
                }
                PMData.AddToQueuePinsList(pinCollection);
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseGetPins: could not get the pins for the following reason", exp, Logs.Error.ErrorsPriority.NotCritical);

                try
                {
                    var item = JsonConvert.DeserializeObject<JArray>(json);

                    Logs.Error.ShowError("error " + (string)item[1], Logs.Error.ErrorsPriority.NotCritical);
        
                }
                catch (Exception exp2)
                {
                    Logs.Error.ShowError("ParseGetPins: could not get the error message", exp2, Logs.Error.ErrorsPriority.NotCritical);
                } 
            }
            //var pin = new PMMapPushpinModel(PMMapPushpinModel.PinsType.PublicMessage, new GeoCoordinate(0, 0));
            //pin.CompleteInitialization("test", "mdlknskdhlr!!!");

        }
    }   
}
