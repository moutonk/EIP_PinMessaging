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
                            var item3 = JsonConvert.DeserializeObject<JObject>(json);
                            PMData.IsSignUpSuccess = Boolean.Parse((string) item3.GetValue("success"));
                            break;

                        case RequestType.GetPins:
                            ParseGetPins(json);
                            break;

                        case RequestType.CreatePin:
                            ParseCreatePin(json);
                            break;

                        case RequestType.ChangePassword:
                            var item4 = JsonConvert.DeserializeObject<JArray>(json);
                            PMData.IsChangePwdSuccess = Boolean.Parse((string) item4[0]);
                            break;

                        case RequestType.ChangeEmail:
                            var item5 = JsonConvert.DeserializeObject<JArray>(json);
                            PMData.IsChangeEmailSuccess = Boolean.Parse((string)item5[0]);
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
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (item.Count == 2)
                {
                    try
                    {
                        if (Convert.ToBoolean(item[0].ToString()) == true)
                        {
                            var pin = JsonConvert.DeserializeObject<PMPinModel>(item[1].ToString());
                            var pinController = new PMPinController(RequestType.CreatePin, null);

                            pinController.CompleteDataMember(pin);
                            pin.ShowPinContent();

                            PMData.AddToQueuePinsList(pin);
                        }
                        else if (Convert.ToBoolean(item[0].ToString()) == false)
                        {
                            Logs.Error.ShowError("DO nothing for now", Logs.Error.ErrorsPriority.NotCritical);
                        }
                    }
                    catch (Exception exp)
                    {
                        Logs.Error.ShowError("ParseCreatePin: could not create the pins for the following reason: ", exp, Logs.Error.ErrorsPriority.NotCritical);
                    }
                }
                else
                {
                    Logs.Error.ShowError("ParseCreatePin: 2 tokens are expected, got " + item.Count, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseCreatePin: could not deserialize the JSON object for the following reason: ", exp, Logs.Error.ErrorsPriority.NotCritical);                    
            }
        }

        public  void ParseGetPins(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (item.Count == 2)
                {
                    try
                    {
                        if (Convert.ToBoolean(item[0].ToString()) == true)
                        {
                            var pinCollection = JsonConvert.DeserializeObject<List<PMPinModel>>(item[1].ToString());
                            var pinController = new PMPinController(RequestType.GetPins, null);

                            foreach (var pmMapPushpinModel in pinCollection)
                            {
                                pinController.CompleteDataMember(pmMapPushpinModel);
                                pmMapPushpinModel.ShowPinContent();

                                //Deployment.Current.Dispatcher.BeginInvoke(() => PMMapPinController.AddPinToMap(pmMapPushpinModel));
                            }
                            PMData.AddToQueuePinsList(pinCollection);
                        }
                        else if (Convert.ToBoolean(item[0].ToString()) == false)
                        {
                            Logs.Error.ShowError("DO nothing for now", Logs.Error.ErrorsPriority.NotCritical);
                               
                        }
                    }
                    catch (Exception exp)
                    {
                        Logs.Error.ShowError("ParseGetPins: could not get the pins for the following reason: ", exp, Logs.Error.ErrorsPriority.NotCritical);
                    }
                }
                else
                {
                    Logs.Error.ShowError("ParseGetPins: 2 tokens are expected, got " + item.Count, Logs.Error.ErrorsPriority.NotCritical);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseGetPins: could not deserialize the JSON object for the following reason: ", exp, Logs.Error.ErrorsPriority.NotCritical);                    
            }
        }
    }   
}
