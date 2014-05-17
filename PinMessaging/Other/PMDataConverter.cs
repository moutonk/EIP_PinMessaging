﻿using System;
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
                            SignUp(json);
                            break;

                        case RequestType.GetPins:
                            ParseGetPins(json);
                            break;

                        case RequestType.CreatePin:
                            ParseCreatePin(json);
                            break;

                        case RequestType.CreatePinMessage:
                            ParseCreatePinMessage(json);
                            break;

                        case RequestType.GetPinMessages:
                            ParseGetPinMessages(json);
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

        private void ParseGetPinMessages(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (Boolean.Parse(item[0].ToString()) == true)
                {
                    switch (item.Count)
                    {
                        case 1:
                            Logs.Error.ShowError("ParseGetPinMessages: 2 tokens are expected, got 1", Logs.Error.ErrorsPriority.NotCritical);
                            break;
                        case 2:
                            var pinCollection = JsonConvert.DeserializeObject<List<PMPinCommentModel>>(item[1].ToString());
                            foreach (var pmMapPushpinModel in pinCollection)
                                pmMapPushpinModel.ShowPinContent();
                            PMData.AddToQueuePinCommentsTmp(pinCollection);
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseGetPinMessages: could not deserialize the JSON object. Return value: " + json, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void ParseCreatePinMessage(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (Boolean.Parse(item[0].ToString()) == false)
                {
                    Logs.Error.ShowError("ParseCreatePinMessage: unknown error", Logs.Error.ErrorsPriority.NotCritical);
                }
                else
                {
                    switch (item.Count)
                    {
                        case 1:
                            Logs.Error.ShowError("ParseCreatePinMessage: 2 tokens are expected, got 1", Logs.Error.ErrorsPriority.NotCritical);
                            break;
                        case 2:
                            var pinCollection = JsonConvert.DeserializeObject<PMPinCommentModel>(item[1].ToString());
                            PMData.AddToQueuePinCommentsTmp(pinCollection);
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseCreatePinMessage: could not deserialize the JSON object. Return value: " + json, Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void SignUp(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);
                PMData.IsSignUpSuccess = Boolean.Parse(item[0].ToString());
            }
            catch (Exception exp)
            {
                PMData.IsSignUpSuccess = false;
                Logs.Error.ShowError("SignUp: could not sign up", Logs.Error.ErrorsPriority.NotCritical);
                Logs.Error.ShowError("EMAIL_EMPTY = 0" + Environment.NewLine + "SIMID_EMPTY = 1" + Environment.NewLine +
                                     "PASSWORD_EMPTY = 2" + Environment.NewLine + "EMAIL_INCORRECT = 3" + Environment.NewLine +
                                     "LOGIN_ALREADY_USE = 4" + Environment.NewLine + "EMAIL_ALREADY_USE = 5", Logs.Error.ErrorsPriority.NotCritical);
            }
        }

        private void ParseCreatePin(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (Boolean.Parse(item[0].ToString()) == false)
                {
                    switch (item.Count)
                    {
                        case 1:
                            Logs.Error.ShowError("ParseCreatePin: not supposed to happen", Logs.Error.ErrorsPriority.NotCritical);
                            break;
                        case 2:
                            Logs.Error.ShowError("ParseCreatePin: error: " + item[1].ToString(), Logs.Error.ErrorsPriority.NotCritical);
                            break;
                        default:
                            Logs.Error.ShowError("ParseCreatePin: error: " + item[1].ToString() + " " + item[2].ToString(), Logs.Error.ErrorsPriority.NotCritical);
                            break;
                    }
                }
                else
                {
                    var pin = JsonConvert.DeserializeObject<PMPinModel>(item[1].ToString());

                    pin.ShowPinContent();
                    PMData.AddToQueuePinsList(pin);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseCreatePin: could not deserialize the JSON object. Return value: " + json, Logs.Error.ErrorsPriority.NotCritical);     
            }
        }

        public  void ParseGetPins(string json)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<JArray>(json);

                if (Boolean.Parse(item[0].ToString()) == false)
                {
                    if (item.Count == 1)
                        Logs.Error.ShowError("ParseGetPins: no pins found", Logs.Error.ErrorsPriority.NotCritical);                
                    else
                        Logs.Error.ShowError("ParseGetPins: error: " + item[1].ToString(), Logs.Error.ErrorsPriority.NotCritical);                
                }
                else
                {
                    var pinCollection = JsonConvert.DeserializeObject<List<PMPinModel>>(item[1].ToString());

                    foreach (var pmMapPushpinModel in pinCollection)
                    {
                        pmMapPushpinModel.ShowPinContent();
                    }
                    PMData.AddToQueuePinsList(pinCollection);
                }
            }
            catch (Exception exp)
            {
                Logs.Error.ShowError("ParseGetPins: could not deserialize the JSON object. Return value: " + json, Logs.Error.ErrorsPriority.NotCritical);
            }
        }
    }   
}
